using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AceRental.Application.Reservations.Dtos;
using AutoMapper;
using AceRental.Domain.Enum;
using System.Text.Json;
using AceRental.Domain.Extensions;
using AceRental.Application.Exceptions;

namespace AceRental.Application.Reservations.Command;

public class UpdateReservationHandler : IRequestHandler<UpdateReservationCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    // private readonly IUnitOfWork _unitOfWork;

    public UpdateReservationHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {

        if (request.Items.Count == 0) 
            throw new NotFoundException($"Pas d'équipements dans la réservation.");

        var reservation = await _context.Reservations
                .Include(r => r.Items)
                .FirstOrDefaultAsync(e => e.Id == request.ReservationId, cancellationToken);
        if (reservation == null) 
            throw new NotFoundException(nameof(Reservation), request.ReservationId);

        // if (reservation.IsContentLocked)
        // {
        //     throw new InvalidOperationException("Cette réservation est verrouillée (Payée ou Facturée). " +
        //         "Veuillez créer un avenant pour toute modification de matériel.");
        // }
        // var immutableStatuses = new[]
        // {
        //     ReservationStatus.Invoiced,
        //     ReservationStatus.Finished,
        //     ReservationStatus.Cancelled,
        //     ReservationStatus.Returned
        // };
        // if (immutableStatuses.Contains(reservation.Status))
        // {
        //     throw new InvalidOperationException($"Impossible de modifier une réservation au statut {reservation.Status}.");
        // }

        var isAvailable = await CheckAvailabilityItems(request.ReservationId, request.StartDate, request.EndDate, request.Items);
        if (!isAvailable) 
            throw new UnavailableQuantityException();

        var historyEntry = new ReservationHistoryDto()
        {
            ReservationId = reservation.Id,
            HistoryType = HistoryType.Content,
            VersionNumber = reservation.CurrentVersion,
            ChangeReason = "Modification des données",
            DataSnapshotJson = JsonSerializer.Serialize(_mapper.Map<ReservationDetailsDto>(reservation))
        };
        _context.ReservationHistorys.Add(_mapper.Map<ReservationHistory>(historyEntry));

        reservation.StartDate = request.StartDate;
        reservation.EndDate = request.EndDate;
        reservation.CurrentVersion++;
        var currentItems = _mapper.Map<List<ReservationItemDto>>(reservation.Items);
        if (HaveItemsChanged(currentItems, request.Items))
        {
            UpdateItems(reservation.Items, request.Items);
            decimal totalAmount = 0;
            foreach (var item in reservation.Items)
            {
                decimal price = 0;
                if (item.EquipmentId.HasValue)
                {
                    var eq = await _context.Equipments.FindAsync(item.EquipmentId);
                    price = eq!.DailyPriceHT;
                }
                else if (item.PackId.HasValue)
                {
                    var pk = await _context.Packs.FindAsync(item.PackId);
                    price = pk!.DailyPriceHT;
                }
                totalAmount += (price * item.Quantity * (reservation.EndDate - reservation.StartDate).Days);
            }
            reservation.TotalHT = totalAmount;
        }

        // Si c'était un Devis (Quote), on peut le repasser en brouillon ou le marquer "Revised"
        // if (reservation.Status == ReservationStatus.Quote)
        // {
        //     // Logique optionnelle : forcer une nouvelle validation
        //     // il faut mettre à jour la date d'expiration de devis en datetime.now
        // }
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> CheckAvailabilityItems(Guid reservationId, DateTime start, DateTime end, List<ReservationItemDto> requestItems)
    {
        var equipmentsToCheck = new Dictionary<Guid, int>();

        foreach (var item in requestItems)
        {
            if (item.EquipmentId.HasValue)
            {
                equipmentsToCheck[item.EquipmentId.Value] = equipmentsToCheck.GetValueOrDefault(item.EquipmentId.Value) + item.Quantity;
            }
            else if (item.PackId.HasValue)
            {
                var packComposition = await _context.Packs
                    .Include(x => x.Items)
                    .Where(pe => pe.Id == item.PackId)
                    .ToListAsync();

                foreach (var pc in packComposition)
                {
                    foreach (var e in pc.Items)
                    {
                        equipmentsToCheck[e.EquipmentId] = equipmentsToCheck.GetValueOrDefault(e.EquipmentId) + (e.Quantity * item.Quantity);
                    }
                }
            }
        }

        foreach (var (eqId, qty) in equipmentsToCheck)
        {
            if (!await CheckAvailability(reservationId, eqId, start, end, qty))
            {
                throw new UnavailableQuantityException(eqId);
            }
        }
        return true;
    }
    private async Task<bool> CheckAvailability(Guid reservationId, Guid equipmentId, DateTime start, DateTime end, int requestedQty)
    {
        // Logique : Somme des quantités louées dans les réservations qui chevauchent ces dates
        var rentedQty = await _context.ReservationItems
            .Where(ri => ri.EquipmentId == equipmentId && ri.ReservationId != reservationId &&
                         ri.Reservation.StartDate < end &&
                         ri.Reservation.EndDate > start)
            .SumAsync(ri => ri.Quantity);

        var totalStock = (await _context.Equipments.FindAsync(equipmentId))?.TotalStock ?? 0;
        return (totalStock - rentedQty) >= requestedQty;
    }
    private bool HaveItemsChanged(List<ReservationItemDto> currentItems, List<ReservationItemDto> newItems)
    {
        if (currentItems.Count != newItems.Count) return true;

        // On crée un dictionnaire : Clé = (IdEquipement ou IdPack) | Valeur = Quantité
        var currentMapping = currentItems.ToDictionary(
            i => i.EquipmentId ?? i.PackId ?? Guid.Empty,
            i => i.Quantity
        );

        foreach (var newItem in newItems)
        {
            var id = newItem.EquipmentId ?? newItem.PackId;
            // Si l'ID n'existe pas dans l'ancienne liste ou si la quantité diffère
            if (id == null || !currentMapping.TryGetValue(id.Value, out int currentQty) || currentQty != newItem.Quantity)
            {
                return true;
            }
        }

        return false;
    }
    private void UpdateItems(List<ReservationItem> currentItems, List<ReservationItemDto> updatedItems)
    {
        // 1. Supprimer les éléments qui ne sont plus dans la nouvelle liste
        var toRemove = currentItems.Where(old => !updatedItems.Any(n =>
            (n.EquipmentId == old.EquipmentId && n.PackId == old.PackId))).ToList();

        foreach (var item in toRemove) currentItems.Remove(item);

        // 2. Mettre à jour ou Ajouter
        foreach (var newItem in updatedItems)
        {
            var existing = currentItems.FirstOrDefault(i =>
                (newItem.EquipmentId.HasValue && i.EquipmentId == newItem.EquipmentId) ||
                (newItem.PackId.HasValue && i.PackId == newItem.PackId));

            if (existing != null)
            {
                existing.Quantity = newItem.Quantity;
                // On peut aussi décider de mettre à jour le prix si c'est un devis non confirmé
            }
            else
            {
                currentItems.Add(new ReservationItem
                {
                    EquipmentId = newItem.EquipmentId,
                    PackId = newItem.PackId,
                    Quantity = newItem.Quantity,
                    UnitPriceAtTimeOfBooking = newItem.UnitPriceAtTimeOfBooking // Prix au moment de l'ajout
                });
            }
        }
    }
}


