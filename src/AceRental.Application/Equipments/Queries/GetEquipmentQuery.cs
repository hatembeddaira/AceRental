using AceRental.Application.Equipments.Dtos;
using MediatR;

namespace AceRental.Application.Equipments.Queries
{
    public record GetEquipmentQuery(
        Guid EquipmentId
    ) : IRequest<EquipmentDetailsDto>; 
}