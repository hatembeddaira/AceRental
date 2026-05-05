using AceRental.Application.Payments.Dtos;
using AceRental.Domain.Entities;
using MediatR;

namespace AceRental.Application.Payments.Queries
{
    public record GetAllPaymentsQuery() : IRequest<IQueryable<PaymentDetailsDto>>; 
}