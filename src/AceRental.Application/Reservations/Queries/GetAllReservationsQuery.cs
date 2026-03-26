using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Entities;
using MediatR;

namespace AceRental.Application.Reservations.Queries
{
    public record GetAllReservationsQuery() : IRequest<List<ReservationDetailsDto>>; 
}