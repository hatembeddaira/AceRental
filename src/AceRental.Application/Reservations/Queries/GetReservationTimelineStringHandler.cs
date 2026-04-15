using System.Data.Common;
using System.Text.Json;
using System.Text.Json.Serialization;
using AceRental.Application.Exceptions;
using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Entities;
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

        public async Task<string> Handle(GetReservationTimelineStringQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                    .FirstOrDefaultAsync(e => e.Id == request.ReservationId, cancellationToken);
            if (reservation == null) 
                throw new NotFoundException(nameof(Reservation), request.ReservationId);

            var history = await _context.ReservationHistorys
                .Where(x => x.ReservationId == request.ReservationId)
                .Select(h => new
                {
                    Date = h.CreatedAt,
                    Status = h.DataSnapshotJson,
                    Type = h.HistoryType.ToString()
                })
                // .Union(
                //     _context.Payments
                //     .Where(x => x.ReservationId == request.ReservationId)
                //     .Select(p => new
                //         {
                //             Date = p.CreatedAt,
                //             Status = p.Type.ToString(),
                //             Type = "Financial"
                //         })
                // )
                .OrderBy(x => x.Date)
                .ToListAsync(cancellationToken);

            // var str = history.Select(h => (JsonSerializer.Deserialize<JsonElement>(h.DataSnapshotJson)).GetProperty("New").GetString() + " (" + h.HistoryType.ToString() + ")")
            // .ToList();

            return string.Join(" => ", history.Select(x => $"{(JsonSerializer.Deserialize<JsonElement>(x.Status)).GetProperty("New").GetString()} ({x.Type})"));
            
            // return string.Join(" => ", str);
        }
    }
}