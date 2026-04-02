using MediatR;

namespace AceRental.Application.Invoices.Command
{
    public record GenerateRentalInvoiceCommand(Guid ReservationId) : IRequest<Guid>;
}