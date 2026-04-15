using AceRental.Application.Clients.Dtos;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Clients.Queries
{
    public class GetAllClientsHandle : IRequestHandler<GetAllClientsQuery, IQueryable<ClientDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllClientsHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IQueryable<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            return  _context.Clients
                .AsNoTracking()
                .ProjectTo<ClientDto>(_mapper.ConfigurationProvider);
        }
    }
}