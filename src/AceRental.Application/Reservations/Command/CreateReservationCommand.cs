using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Reservations.Command;

public record CreateReservationCommand(
    Guid ClientId,
    DateTime StartDate,
    DateTime EndDate,
    Workflow Workflow,
    List<ReservationEquipmentsDto>? Equipments,
    List<ReservationPacksDto>? Packs,
    List<ReservationServicesDto>? Services
    ) : IRequest<ReservationDetailsDto>;