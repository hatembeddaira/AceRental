using MediatR;

namespace AceRental.Application.Invoices.Command
{
    public record GeneratePartiallyInvoiceCommand(Guid ReservationId, Guid PaymentId) : IRequest<Guid>;
}