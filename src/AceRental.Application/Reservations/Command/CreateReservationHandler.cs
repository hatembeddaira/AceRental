using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AceRental.Application.Reservations.Dtos;
using AutoMapper;
using AceRental.Domain.Enum;

namespace AceRental.Application.Reservations.Command;

public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, Guid>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateReservationHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {

        if (request.Items.Count == 0) throw new Exception($"Pas d'équipements dans la réservation.");

        var isAvailable = await CheckAvailabilityItems(request.StartDate, request.EndDate, request.Items);
        if (!isAvailable) throw new Exception($"Matériel  indisponible.");

        var reservation = new ReservationDetailsDto
        {
            Id = Guid.NewGuid(),
            ReservationNumber = await GenerateReservationNumber(),
            ClientId = request.ClientId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            LogisticStatus = request.Workflow == Workflow.B2C ? LogisticStatus.Confirmed : LogisticStatus.Draft,
            FinancialStatus = FinancialStatus.Unpaid,
            Workflow = request.Workflow,
            TotalHT = 0 
        };

        foreach (var item in request.Items)
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

            reservation.Items.Add(new ReservationItemDto
            {
                EquipmentId = item.EquipmentId,
                ReservationId = reservation.Id,
                PackId = item.PackId,
                Quantity = item.Quantity,
                UnitPriceAtTimeOfBooking = price
            });

            reservation.TotalHT += (price * item.Quantity * (reservation.EndDate - reservation.StartDate).Days);
        }

        _context.Reservations.Add(_mapper.Map<Reservation>(reservation));
        await _context.SaveChangesAsync(cancellationToken);
        return reservation.Id;
    }
    private async Task<bool> CheckAvailabilityItems(DateTime start, DateTime end, List<ReservationItemDto> requestItems)
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
                // On récupère la composition du pack pour savoir quels équipements vérifier
                var packComposition = await _context.Packs
                    .Include(x => x.Items)
                    .Where(pe => pe.Id == item.PackId)
                    .FirstOrDefaultAsync();

                foreach (var e in packComposition?.Items!)
                {
                    equipmentsToCheck[e.EquipmentId] = equipmentsToCheck.GetValueOrDefault(e.EquipmentId) + (e.Quantity * item.Quantity);
                }
            }
        }

        // On vérifie la disponibilité pour chaque équipement unique identifié
        foreach (var (eqId, qty) in equipmentsToCheck)
        {
            if (!await CheckAvailability(eqId, start, end, qty))
            {
                throw new Exception($"Le matériel (ID: {eqId}) n'est plus disponible en quantité suffisante pour ces dates.");
            }
        }
        return true;
    }
    private async Task<bool> CheckAvailability(Guid equipmentId, DateTime start, DateTime end, int requestedQty)
    {
        // Logique : Somme des quantités louées dans les réservations qui chevauchent ces dates
        var rentedQty = await _context.ReservationItems
            .Where(ri => ri.EquipmentId == equipmentId &&
                         ri.Reservation.StartDate < end &&
                         ri.Reservation.EndDate > start)
            .SumAsync(ri => ri.Quantity);

        var totalStock = (await _context.Equipments.FindAsync(equipmentId))?.TotalStock ?? 0;

        return (totalStock - rentedQty) >= requestedQty;
    }
    private async Task<int> GenerateReservationNumber()
    {
        // Logique : Somme des quantités louées dans les réservations qui chevauchent ces dates
        var lastReservationNumber = await _context.Reservations
            .IgnoreQueryFilters()
            .Where(ri => ri.CreatedAt.Year == DateTime.Now.Year)
            .OrderByDescending(ri => ri.ReservationNumber)
            .Select(x => x.ReservationNumber).FirstOrDefaultAsync();

        if (lastReservationNumber == 0)
            lastReservationNumber = DateTime.Now.Year * 1000;

        return lastReservationNumber + 1;
    }
}


