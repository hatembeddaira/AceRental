using System.Data.Common;
using System.Text.Json;
using AceRental.Application.Reservations.Dtos;
using AceRental.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Reservations.Queries
{
    public class GetReservationTimelineStringHandler : IRequestHandler<GetReservationTimelineStringQuery, string>
    {
        private readonly ApplicationDbContext _context;
        
        public GetReservationTimelineStringHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(GetReservationTimelineStringQuery request, CancellationToken ct)
        {
            var history = await _context.ReservationHistorys
                .Where(h => h.ReservationId == request.ReservationId && h.ChangeReason == "Changement de statut")
                .OrderBy(h => h.CreatedAt) // On veut le plus récent en haut
                .ToListAsync(ct);

            var str = history.Select(h => (JsonSerializer.Deserialize<JsonElement>(h.DataSnapshotJson)).GetProperty("New").GetString() + " (" + h.HistoryType.ToString() + ")")
            .ToList();
            return string.Join(" => ", str);
        }
    }
}