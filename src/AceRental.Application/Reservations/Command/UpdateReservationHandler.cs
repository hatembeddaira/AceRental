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
using FluentValidation.Results;
using AceRental.Infrastructure.Persistence.Repositories;

namespace AceRental.Application.Reservations.Command;

public class UpdateReservationHandler : IRequestHandler<UpdateReservationCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IReservationEquipmentsRepository _reservationRepo;

    public UpdateReservationHandler(ApplicationDbContext context, IMapper mapper, IReservationEquipmentsRepository reservationRepo)
    {
        _context = context;
        _mapper = mapper;
        _reservationRepo = reservationRepo;
    }

    public async Task<bool> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {

        if (request.EndDate != null && 
            request.StartDate != null && 
            ((DateTime)request.EndDate - (DateTime)request.StartDate).Days <= 0)
            throw new ValidationException(new List<ValidationFailure>
            {
                new ValidationFailure(nameof(request.EndDate), "La date de fin doit être postérieure à la date de début.")
            });
        if (request.Equipments?.Count == 0 && request.Packs?.Count == 0 && request.Services?.Count == 0)
            throw new NotFoundException($"Pas d'éléments dans la réservation.");

        // Charger sans tracker pour éviter les problèmes de concurrence
        var reservation = await _context.Reservations
                .Include(r => r.Equipments)
                .Include(r => r.Packs)
                .Include(r => r.Services)
                .FirstOrDefaultAsync(e => e.Id == request.ReservationId, cancellationToken);
        if (reservation == null)
            throw new NotFoundException(nameof(Reservation), request.ReservationId);

        var isAvailable = await CheckAvailabilityItems(
            request,
            request.StartDate ?? reservation.StartDate,
            request.EndDate ?? reservation.EndDate, cancellationToken);
        if (!isAvailable)
            throw new UnavailableQuantityException();

        // Créer l'entrée d'historique avant toute modification
        var historyEntry = new ReservationHistoryDto()
        {
            ReservationId = reservation.Id,
            HistoryType = HistoryType.Content,
            VersionNumber = reservation.CurrentVersion,
            ChangeReason = "Modification des données",
            DataSnapshotJson = JsonSerializer.Serialize(_mapper.Map<ReservationDetailsDto>(reservation))
        };
        _context.ReservationHistorys.Add(_mapper.Map<ReservationHistory>(historyEntry));

        // Gérer les modifications d'items en premier
        if (request.Equipments != null && request.Equipments.Any())
        {
            var currentItems = reservation.Equipments.Select(i => new ReservationEquipmentsDto 
            { 
                EquipmentId = i.EquipmentId,
                Quantity = i.Quantity,
                UnitPriceAtTimeOfBooking = i.UnitPriceAtTimeOfBooking,
                ReservationId = i.ReservationId
            }).ToList();

            if (HaveReservationEquipmentsChanged(currentItems, request.Equipments))
            {
                await UpdateReservationEquipmentsAsync(reservation, request.Equipments, cancellationToken);
            }
        }
        if (request.Packs != null && request.Packs.Any())
        {
            var currentItems = reservation.Packs.Select(i => new ReservationPacksDto 
            { 
                PackId = i.PackId,
                Quantity = i.Quantity,
                UnitPriceAtTimeOfBooking = i.UnitPriceAtTimeOfBooking,
                ReservationId = i.ReservationId
            }).ToList();

            if (HaveReservationPacksChanged(currentItems, request.Packs))
            {
                await UpdateReservationPacksAsync(reservation, request.Packs, cancellationToken);
            }
        }
        if (request.Services != null && request.Services.Any())
        {
            var currentItems = reservation.Services.Select(i => new ReservationServicesDto 
            { 
                ServiceId = i.ServiceId,
                Quantity = i.Quantity,
                UnitPriceAtTimeOfBooking = i.UnitPriceAtTimeOfBooking,
                ReservationId = i.ReservationId
            }).ToList();

            if (HaveReservationServicesChanged(currentItems, request.Services))
            {
                await UpdateReservationServicesAsync(reservation, request.Services, cancellationToken);
            }
        }

        // Modifier la réservation
        reservation.StartDate = request.StartDate != null ? request.StartDate.Value : reservation.StartDate;
        reservation.EndDate = request.EndDate != null ? request.EndDate.Value : reservation.EndDate;
        reservation.CurrentVersion++;
        
        reservation.TotalHT = reservation.Equipments.Sum(e => e.Quantity * e.UnitPriceAtTimeOfBooking) +
                              reservation.Packs.Sum(p => p.Quantity * p.UnitPriceAtTimeOfBooking) +
                              reservation.Services.Sum(s => s.Quantity * s.UnitPriceAtTimeOfBooking);

        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    private async Task<bool> CheckAvailabilityItems(UpdateReservationCommand request, DateTime start, DateTime end, CancellationToken cancellationToken)
    {
        var equipmentsToCheck = new Dictionary<Guid, int>();

        foreach (var item in request.Equipments!)
        {
            equipmentsToCheck[item.EquipmentId] = equipmentsToCheck.GetValueOrDefault(item.EquipmentId) + item.Quantity;
        }
        foreach (var item in request.Packs!)
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

        // On vérifie la disponibilité pour chaque équipement unique identifié
        foreach (var (eqId, qty) in equipmentsToCheck)
        {
            if (!await CheckAvailability(request.ReservationId, eqId, start, end, qty, cancellationToken))
            {
                throw new UnavailableQuantityException(eqId);
            }
        }
        return true;
    }    
    private async Task<bool> CheckAvailability(Guid reservationId, Guid equipmentId, DateTime start, DateTime end, int requestedQty, CancellationToken cancellationToken)
    {
        // Logique : Somme des quantités louées dans les réservations qui chevauchent ces dates
        var rentedQty = await _reservationRepo.GetTotalRentedQuantityAsync(reservationId, equipmentId, start, end, cancellationToken);
        var totalStock = (await _context.Equipments.FindAsync(equipmentId))?.TotalStock ?? 0;
        return (totalStock - rentedQty) >= requestedQty;
    }
    private bool HaveReservationEquipmentsChanged(List<ReservationEquipmentsDto> currentItems, List<ReservationEquipmentsDto> newItems)
    {
        if (currentItems.Count != newItems.Count) return true;

        // On crée un dictionnaire : Clé = (IdEquipement ou IdPack) | Valeur = Quantité
        var currentMapping = currentItems.ToDictionary(
            i => i.EquipmentId,
            i => i.Quantity
        );

        foreach (var newItem in newItems)
        {
            // Si l'ID n'existe pas dans l'ancienne liste ou si la quantité diffère
            if (!currentMapping.TryGetValue(newItem.EquipmentId, out int currentQty) || currentQty != newItem.Quantity)
            {
                return true;
            }
        }

        return false;
    }
    private bool HaveReservationPacksChanged(List<ReservationPacksDto> currentItems, List<ReservationPacksDto> newItems)
    {
        if (currentItems.Count != newItems.Count) return true;

        // On crée un dictionnaire : Clé = (IdEquipement ou IdPack) | Valeur = Quantité
        var currentMapping = currentItems.ToDictionary(
            i => i.PackId,
            i => i.Quantity
        );

        foreach (var newItem in newItems)
        {
            // Si l'ID n'existe pas dans l'ancienne liste ou si la quantité diffère
            if (!currentMapping.TryGetValue(newItem.PackId, out int currentQty) || currentQty != newItem.Quantity)
            {
                return true;
            }
        }

        return false;
    }
    private bool HaveReservationServicesChanged(List<ReservationServicesDto> currentItems, List<ReservationServicesDto> newItems)
    {
        if (currentItems.Count != newItems.Count) return true;

        // On crée un dictionnaire : Clé = (IdEquipement ou IdPack) | Valeur = Quantité
        var currentMapping = currentItems.ToDictionary(
            i => i.ServiceId,
            i => i.Quantity
        );

        foreach (var newItem in newItems)
        {
            // Si l'ID n'existe pas dans l'ancienne liste ou si la quantité diffère
            if (!currentMapping.TryGetValue(newItem.ServiceId, out int currentQty) || currentQty != newItem.Quantity)
            {
                return true;
            }
        }

        return false;
    }
    private async Task UpdateReservationEquipmentsAsync(Reservation reservation, List<ReservationEquipmentsDto> updatedEquipments, CancellationToken cancellationToken)
    {
        // 1. Supprimer les items qui ne sont plus dans la nouvelle liste
        var toRemove = reservation.Equipments.Where(old => !updatedEquipments.Any(n => n.EquipmentId == old.EquipmentId)).ToList();
        foreach (var item in toRemove)
        {
            _context.ReservationEquipments.Remove(item);
        }

        // 2. Préparation des prix
        var allItemIds = updatedEquipments.Select(i => i.EquipmentId).ToList();
        var equipments = await _context.Equipments.Where(e => allItemIds.Contains(e.Id)).ToDictionaryAsync(e => e.Id, cancellationToken);
        // decimal totalAmount = 0;

        // 3. Mise à jour ou ajout des items
        foreach (var item in updatedEquipments)
        {
            var existing = reservation.Equipments.FirstOrDefault(i => i.EquipmentId == item.EquipmentId);
            decimal unitPrice = 0;

            if (equipments.TryGetValue(item.EquipmentId, out var eq)) unitPrice = eq.DailyPriceHT;

            if (existing != null)
            {
                existing.Quantity = item.Quantity;
                existing.UnitPriceAtTimeOfBooking = unitPrice;
            }
            else
            {
                var newItem = new ReservationEquipments
                {
                    EquipmentId = item.EquipmentId,
                    Quantity = item.Quantity,
                    UnitPriceAtTimeOfBooking = unitPrice,
                    ReservationId = reservation.Id
                };
                _context.ReservationEquipments.Add(newItem);
            }

            // totalAmount += (unitPrice * item.Quantity * (reservation.EndDate - reservation.StartDate).Days);
        }

        // reservation.TotalHT = totalAmount;
    }
    private async Task UpdateReservationPacksAsync(Reservation reservation, List<ReservationPacksDto> updatedPacks, CancellationToken cancellationToken)
    {
        // 1. Supprimer les items qui ne sont plus dans la nouvelle liste
        var toRemove = reservation.Packs.Where(old => !updatedPacks.Any(n => n.PackId == old.PackId)).ToList();
        foreach (var item in toRemove)
        {
            _context.ReservationPacks.Remove(item);
        }

        // 2. Préparation des prix
        var allItemIds = updatedPacks.Select(i => i.PackId).ToList();
        var packs = await _context.Packs.Where(p => allItemIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id, cancellationToken);
        
        // decimal totalAmount = 0;

        // 3. Mise à jour ou ajout des items
        foreach (var item in updatedPacks)
        {
            var existing = reservation.Packs.FirstOrDefault(i => i.PackId == item.PackId);
            decimal unitPrice = 0;

            if (packs.TryGetValue(item.PackId, out var p)) unitPrice = p.DailyPriceHT;

            if (existing != null)
            {
                existing.Quantity = item.Quantity;
                existing.UnitPriceAtTimeOfBooking = unitPrice;
            }
            else
            {
                var newItem = new ReservationPacks
                {
                    PackId = item.PackId,
                    Quantity = item.Quantity,
                    UnitPriceAtTimeOfBooking = unitPrice,
                    ReservationId = reservation.Id
                };
                _context.ReservationPacks.Add(newItem);
            }

            // totalAmount += (unitPrice * item.Quantity * (reservation.EndDate - reservation.StartDate).Days);
        }

        // reservation.TotalHT = totalAmount;
    }  
    private async Task UpdateReservationServicesAsync(Reservation reservation, List<ReservationServicesDto> updatedServices, CancellationToken cancellationToken)
    {
        // 1. Supprimer les items qui ne sont plus dans la nouvelle liste
        var toRemove = reservation.Services.Where(old => !updatedServices.Any(n => n.ServiceId == old.ServiceId)).ToList();
        foreach (var item in toRemove)
        {
            _context.ReservationServices.Remove(item);
        }

        // 2. Préparation des prix
        var allItemIds = updatedServices.Select(i => i.ServiceId).ToList();
        var equipments = await _context.Equipments.Where(e => allItemIds.Contains(e.Id)).ToDictionaryAsync(e => e.Id, cancellationToken);
        var packs = await _context.Packs.Where(p => allItemIds.Contains(p.Id)).ToDictionaryAsync(p => p.Id, cancellationToken);
        var services = await _context.Services.Where(s => allItemIds.Contains(s.Id)).ToDictionaryAsync(s => s.Id, cancellationToken);

        // decimal totalAmount = 0;

        // 3. Mise à jour ou ajout des items
        foreach (var item in updatedServices)
        {
            var existing = reservation.Services.FirstOrDefault(i => i.ServiceId == item.ServiceId);
            decimal unitPrice = 0;

            if (services.TryGetValue(item.ServiceId, out var s)) unitPrice = s.DailyPriceHT;

            if (existing != null)
            {
                existing.Quantity = item.Quantity;
                existing.UnitPriceAtTimeOfBooking = unitPrice;
            }
            else
            {
                var newItem = new ReservationServices
                {
                    ServiceId = item.ServiceId,
                    Quantity = item.Quantity,
                    UnitPriceAtTimeOfBooking = unitPrice,
                    ReservationId = reservation.Id
                };
                _context.ReservationServices.Add(newItem);
            }

            // totalAmount += (unitPrice * item.Quantity * (reservation.EndDate - reservation.StartDate).Days);
        }

        // reservation.TotalHT = totalAmount;
    }

       
}


