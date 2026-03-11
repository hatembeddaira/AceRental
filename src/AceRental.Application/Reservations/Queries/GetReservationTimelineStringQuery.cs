using AceRental.Application.Reservations.Dtos;
using MediatR;

namespace AceRental.Application.Reservations.Queries
{
   public record GetReservationTimelineStringQuery(
        Guid ReservationId
    ) : IRequest<string>; 
}