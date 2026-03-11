using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Reservations.Command
{
    public record ChangeFinancialStatusCommand(
    Guid ReservationId,
    FinancialStatus Status
    ) : IRequest<bool>;
}