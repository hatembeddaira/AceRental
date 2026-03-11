using MediatR;

namespace AceRental.Application.Equipments.Queries
{
    public record GetEquipmentAvailabilityQuery(
        Guid EquipmentId,
        DateTime StartDate,
        DateTime EndDate
    ) : IRequest<int>; // Retourne la quantité restante disponible
}