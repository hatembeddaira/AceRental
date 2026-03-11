using System.Data.Common;
using System.Text.Json;
using AceRental.Application.Reservations.Dtos;
using AceRental.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Reservations.Queries
{
    public class GetReservationTimelineHandler : IRequestHandler<GetReservationTimelineQuery, List<ReservationTimelineDto>>
    {
        private readonly ApplicationDbContext _context;
        
        public GetReservationTimelineHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReservationTimelineDto>> Handle(GetReservationTimelineQuery request, CancellationToken ct)
        {
            var history = await _context.ReservationHistorys
                .Where(h => h.ReservationId == request.ReservationId)
                .OrderByDescending(h => h.CreatedAt) // On veut le plus récent en haut
                .ToListAsync(ct);

            return history.Select(h => new ReservationTimelineDto()
            {
                Id = h.Id,
                Date = h.CreatedAt,
                HistoryType = h.HistoryType,
                Title = h.ChangeReason ?? "",
                User = h.CreatedBy ?? "",
                Details = JsonSerializer.Deserialize<object>(h.DataSnapshotJson)
            }
            ).ToList();
        }
    }
}