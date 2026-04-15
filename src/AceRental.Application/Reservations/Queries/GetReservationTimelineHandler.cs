using System.Data.Common;
using System.Text.Json;
using AceRental.Application.Exceptions;
using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Entities;
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

        public async Task<List<ReservationTimelineDto>> Handle(GetReservationTimelineQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                    .FirstOrDefaultAsync(e => e.Id == request.ReservationId, cancellationToken);
            if (reservation == null) 
                throw new NotFoundException(nameof(Reservation), request.ReservationId);
                
            var history = await _context.ReservationHistorys
                .Where(h => h.ReservationId == request.ReservationId)
                .OrderByDescending(h => h.CreatedAt) // On veut le plus récent en haut
                .ToListAsync(cancellationToken);

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