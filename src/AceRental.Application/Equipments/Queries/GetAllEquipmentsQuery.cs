using AceRental.Application.Equipments.Dtos;
using AceRental.Domain.Entities;
using MediatR;

namespace AceRental.Application.Equipments.Queries
{
    public record GetAllEquipmentsQuery() : IRequest<IQueryable<EquipmentDetailsDto>>; 
}