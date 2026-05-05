using Microsoft.EntityFrameworkCore;
using AceRental.Infrastructure.Persistence;
using System.Net.NetworkInformation;

namespace AceRental.Infrastructure.Persistence.Repositories
{
    public interface IReservationEquipmentsRepository
    {
        Task<int> GetTotalRentedQuantityAsync(Guid? reservationId, Guid equipmentId, DateTime start, DateTime end, CancellationToken ct);
    }
    public class ReservationEquipmentsRepository : IReservationEquipmentsRepository
    {
        private readonly ApplicationDbContext _context;

        public ReservationEquipmentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalRentedQuantityAsync(Guid? reservationId, Guid equipmentId, DateTime start, DateTime end, CancellationToken ct)
        {
            bool hasReservationId = reservationId != null;
            // 1. Quantité louée directement (équipement seul)
            var directQuantity = await _context.ReservationEquipments
                .AsNoTracking()
                .Where(ri => ri.EquipmentId == equipmentId && (!hasReservationId || ri.ReservationId != reservationId) &&
                             ri.Reservation.StartDate < end &&
                             ri.Reservation.EndDate > start)
                .Select(ri => (int?)ri.Quantity)
                .SumAsync(ct) ?? 0;

            // 2. Quantité louée via des packs
            var packQuantity = await _context.ReservationPacks
                .AsNoTracking()
                .Where(rp => rp.Reservation.StartDate < end && rp.Reservation.EndDate > start)
                .Join(_context.PackItems,
                      rp => rp.PackId,
                      pi => pi.PackId,
                      (rp, pi) => new { rp, pi })
                .Where(joined => joined.pi.EquipmentId == equipmentId && (!hasReservationId || joined.rp.ReservationId != reservationId))
                .Select(joined => (int?)(joined.rp.Quantity * joined.pi.Quantity))
                .SumAsync(ct) ?? 0;

            // 3. Somme totale
            return directQuantity + packQuantity;
        }    
    }
}