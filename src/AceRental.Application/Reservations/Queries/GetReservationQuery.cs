using AceRental.Application.Reservations.Dtos;
using MediatR;

namespace AceRental.Application.Reservations.Queries
{
    public record GetReservationQuery(
        Guid ReservationId
    ) : IRequest<ReservationDto>; 
}