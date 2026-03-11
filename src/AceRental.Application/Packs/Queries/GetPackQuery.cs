using AceRental.Application.Packs.Dtos;
using MediatR;

namespace AceRental.Application.Packs.Queries
{
    public record GetPackQuery(
        Guid PackId
    ) : IRequest<PackDetailsDto>; 
}