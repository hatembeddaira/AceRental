using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Reservations.Command;

public record CreateReservationCommand(
    Guid ClientId,
    Guid ReservationId,
    DateTime StartDate,
    DateTime EndDate,
    Workflow Workflow,
    List<ReservationItemDto> Items
    ) : IRequest<Guid>;

//public record ReservationItemDto(Guid? EquipmentId, Guid? PackId, int Quentity);