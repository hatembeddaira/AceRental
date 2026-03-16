using AceRental.Application.Clients.Dtos;
using MediatR;

namespace AceRental.Application.Clients.Queries
{
    public record GetClientQuery(
        Guid ClientId
    ) : IRequest<ClientDto>; 
}