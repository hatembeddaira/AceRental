using AceRental.Application.Packs.Dtos;
using MediatR;

namespace AceRental.Application.Packs.Queries
{
    public record GetAllPacksQuery() : IRequest<IQueryable<PackDetailsDto>>; 
}