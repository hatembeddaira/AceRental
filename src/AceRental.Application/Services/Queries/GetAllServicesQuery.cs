using AceRental.Application.Services.Dtos;
using MediatR;

namespace AceRental.Application.Services.Queries
{
    public record GetAllServicesQuery() : IRequest<IQueryable<ServiceDto>>; 
}