using AceRental.Application.Reservations.Dtos;
using MediatR;

namespace AceRental.Application.Reservations.Queries
{
   public record GetReservationTimelineQuery(
        Guid ReservationId
    ) : IRequest<List<ReservationTimelineDto>>; 
}