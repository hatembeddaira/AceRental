using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Reservations.Command
{
    public record ChangeLogisticStatusCommand(
    Guid ReservationId,
    LogisticStatus Status
    ) : IRequest<bool>;
    // public record ChangeLogisticStatusCommand(
    // Guid ReservationId,
    // FinancialStatus Status
    // ) : IRequest<bool>;
}