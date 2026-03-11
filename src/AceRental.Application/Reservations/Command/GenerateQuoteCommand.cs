using MediatR;

namespace AceRental.Application.Reservations.Command;

public record GenerateQuoteCommand(Guid ReservationId) : IRequest<Guid>;