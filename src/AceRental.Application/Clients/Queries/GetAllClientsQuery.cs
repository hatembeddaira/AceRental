using AceRental.Application.Clients.Dtos;
using AceRental.Domain.Entities;
using MediatR;

namespace AceRental.Application.Clients.Queries
{
    public record GetAllClientsQuery() : IRequest<IQueryable<ClientDto>>; 
}