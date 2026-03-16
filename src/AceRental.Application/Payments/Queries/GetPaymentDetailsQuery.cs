using AceRental.Application.Payments.Dtos;
using MediatR;

namespace AceRental.Application.Payments.Queries
{
    public record GetPaymentDetailsQuery(
        Guid PaymentId
    ) : IRequest<PaymentDto>; 
}