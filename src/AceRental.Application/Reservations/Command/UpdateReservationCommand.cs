using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Reservations.Command;

public record UpdateReservationCommand(
    Guid ReservationId,
    DateTime StartDate,
    DateTime EndDate,
    List<ReservationItemDto> Items
    ) : IRequest<bool>;

//public record ReservationItemDto(Guid? EquipmentId, Guid? PackId, int Quentity);