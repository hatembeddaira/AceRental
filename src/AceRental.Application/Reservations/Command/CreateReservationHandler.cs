using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AceRental.Application.Reservations.Dtos;
using AutoMapper;
using AceRental.Domain.Enum;
using AceRental.Application.Exceptions;
using AceRental.Infrastructure.Persistence.Repositories;
using FluentValidation.Results;

namespace AceRental.Application.Reservations.Command;

public class CreateReservationHandler : IRequestHandler<CreateReservationCommand, ReservationDetailsDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IReservationEquipmentsRepository _reservationRepo;

    public CreateReservationHandler(ApplicationDbContext context, IMapper mapper, IReservationEquipmentsRepository reservationRepo)
    {
        _context = context;
        _mapper = mapper;
        _reservationRepo = reservationRepo;
    }

    private async Task<Dictionary<Guid, int>> GetlstEquipments(CreateReservationCommand request, List<Pack> packComposition, CancellationToken cancellationToken)
    {
        Dictionary<Guid, int> lstEquipments = (request.Equipments?.ToDictionary(e => e.EquipmentId, e => e.Quantity) ?? new Dictionary<Guid, int>());       
        lstEquipments.Concat(packComposition.SelectMany(p => p.Items.Select(i => new KeyValuePair<Guid, int>(i.EquipmentId, i.Quantity * (request.Packs?.FirstOrDefault(p => p.PackId == i.PackId)?.Quantity ?? 0)))))
                     .GroupBy(kv => kv.Key)
                     .ToDictionary(g => g.Key, g => g.Sum(kv => kv.Value));
        
        return lstEquipments;
    }
    public async Task<ReservationDetailsDto> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        if ((request.EndDate - request.StartDate).Days <= 0)
            throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.EndDate), "La date de fin doit être postérieure à la date de début.")
            });
        if (request.Equipments?.Count == 0 && request.Packs?.Count == 0 && request.Services?.Count == 0)
            throw new NotFoundException($"Pas d'éléments dans la réservation.");

        var lstIdPacks = request.Packs?.Select(p => p.PackId) ?? new List<Guid>();
        var packComposition = await _context.Packs
                .Include(x => x.Items)
                .Where(pe => lstIdPacks.Contains(pe.Id))
                .ToListAsync();
                
        var lstEquipments = await GetlstEquipments(request, packComposition, cancellationToken);
        var isAvailable = await CheckAvailabilityItems(lstEquipments, request.StartDate, request.EndDate, cancellationToken);
        if (!isAvailable)
            throw new UnavailableQuantityException();

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

        Dictionary<Guid, Equipment> equipmentsInDb = await _context.Equipments
            .Where(e => lstEquipments.Select(x => x.Key).Contains(e.Id))
            .ToDictionaryAsync(e => e.Id, e => e);
        

        var lstIdServices = request.Services?.Select(s => s.ServiceId) ?? new List<Guid>();
        var servicesInDb = await _context.Services
            .Where(s => lstIdServices.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s);
        
        if (request.Equipments != null)
        {
            foreach (var item in request.Equipments)
            {
                reservation.Equipments.Add(new ReservationEquipmentsDto
                {
                    EquipmentId = item.EquipmentId,
                    ReservationId = reservation.Id,
                    Quantity = item.Quantity,
                    UnitPriceAtTimeOfBooking = equipmentsInDb[item.EquipmentId]?.DailyPriceHT ?? 0
                });
            }
        }
        if (request.Packs != null)
        {
            foreach (var item in request.Packs)
            {
                reservation.Packs.Add(new ReservationPacksDto
                {
                    PackId = item.PackId,
                    ReservationId = reservation.Id,
                    Quantity = item.Quantity,
                    UnitPriceAtTimeOfBooking = packComposition.FirstOrDefault(p => p.Id == item.PackId)?.DailyPriceHT ?? 0
                });
            }
        }
        if (request.Services != null)
        {
            foreach (var item in request.Services)
            {
                reservation.Services.Add(new ReservationServicesDto
                {
                    ServiceId = item.ServiceId,
                    ReservationId = reservation.Id,
                    Quantity = item.Quantity,
                    UnitPriceAtTimeOfBooking = servicesInDb[item.ServiceId]?.DailyPriceHT ?? 0
                });
            }
        }

        reservation.TotalHT = reservation.Equipments.Sum(e => e.Quantity * e.UnitPriceAtTimeOfBooking) +
                              reservation.Packs.Sum(p => p.Quantity * p.UnitPriceAtTimeOfBooking) +
                              reservation.Services.Sum(s => s.Quantity * s.UnitPriceAtTimeOfBooking);

        _context.Reservations.Add(_mapper.Map<Reservation>(reservation));
        await _context.SaveChangesAsync(cancellationToken);
        return reservation;
    }
    private async Task<bool> CheckAvailabilityItems(Dictionary<Guid, int> lstEquipments, DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        // var equipmentsToCheck = new Dictionary<Guid, int>();

        // foreach (var item in lstEquipments)
        // {
        //     equipmentsToCheck[item.EquipmentId] = equipmentsToCheck.GetValueOrDefault(item.EquipmentId) + item.Quantity;
        // }
        // foreach (var item in request.Packs)
        // {
        //     // On récupère la composition du pack pour savoir quels équipements vérifier
        //     var packComposition = await _context.Packs
        //         .Include(x => x.Items)
        //         .Where(pe => pe.Id == item.PackId)
        //         .FirstOrDefaultAsync();

        //     foreach (var e in packComposition?.Items!)
        //     {
        //         equipmentsToCheck[e.EquipmentId] = equipmentsToCheck.GetValueOrDefault(e.EquipmentId) + (e.Quantity * item.Quantity);
        //     }
        // }

        // On vérifie la disponibilité pour chaque équipement unique identifié
        foreach (var (eqId, qty) in lstEquipments)
        {
            if (!await CheckAvailability(eqId, start, end, qty, cancellationToken))
            {
                throw new UnavailableQuantityException(eqId);
            }
        }
        return true;
    }
    private async Task<bool> CheckAvailability(Guid ItemId, DateTime start, DateTime end, int requestedQty, CancellationToken cancellationToken)
    {
        var rentedQty = await _reservationRepo.GetTotalRentedQuantityAsync(null, ItemId, start, end, cancellationToken);

        var totalStock = (await _context.Equipments.FindAsync(ItemId))?.TotalStock ?? 0;

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


