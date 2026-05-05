using MediatR;

namespace AceRental.Application.Quotes.Command;

public record GenerateQuoteCommand() : IRequest<Guid>
{
    public Guid ReservationId { get; set; }
}