using AceRental.Application.Quotes.Dtos;
using MediatR;

namespace AceRental.Application.Quotes.Command;

public record CancelQuoteCommand() : IRequest<bool>
{
    public Guid QuoteId { get; set; }
}