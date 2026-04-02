using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Infrastructure.Persistence;
using AceRental.Domain.Enum;
using AceRental.Application.Exceptions;
using AceRental.Domain.Entities;

namespace AceRental.Application.Equipments.Queries
{
    public class GetEquipmentAvailabilityHandler : IRequestHandler<GetEquipmentAvailabilityQuery, int>
    {
        private readonly ApplicationDbContext _context;

        public GetEquipmentAvailabilityHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(GetEquipmentAvailabilityQuery request, CancellationToken cancellationToken)
        {
            // 1. Récupérer le stock total de l'équipement
            var equipment = await _context.Equipments
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.EquipmentId, cancellationToken);

            if (equipment == null) 
                throw new NotFoundException(nameof(Equipment), request.EquipmentId);

            // 2. Calculer la somme des quantités déjà réservées sur cette période
            // Une réservation chevauche si (Start1 < End2) AND (End1 > Start2)
            var rentedQuantity = await _context.ReservationItems
                .Where(ri => ri.EquipmentId == request.EquipmentId)
                .Where(ri => ri.Reservation.LogisticStatus != LogisticStatus.Cancelled)
                .Where(ri => ri.Reservation.StartDate < request.EndDate && ri.Reservation.EndDate > request.StartDate)
                .SumAsync(ri => ri.Quantity, cancellationToken);

            // 3. Retourner la différence
            var availability = equipment.TotalStock - rentedQuantity;
            return availability < 0 ? 0 : availability;
        }
    }
}