using AceRental.Application.Payments.Dtos;
using AceRental.Domain.Entities;
using MediatR;

namespace AceRental.Application.Payments.Queries
{
    public record GetAllPaymentsQuery(
        Guid ReservationId
    ) : IRequest<List<PaymentDto>>; 
}