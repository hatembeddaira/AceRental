using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Reservations.Command;

public record CreateReservationCommand(
    Guid ClientId,
    DateTime StartDate,
    DateTime EndDate,
    Workflow Workflow,
    List<ReservationEquipmentsCommandeDto>? Equipments,
    List<ReservationPacksCommandeDto>? Packs,
    List<ReservationServicesCommandeDto>? Services
    ) : IRequest<ReservationDetailsDto>;