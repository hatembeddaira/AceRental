using AceRental.Application.Clients.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Clients.Queries
{
    public class GetAllClientsHandle : IRequestHandler<GetAllClientsQuery, List<ClientDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllClientsHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClientDto>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            // 1. Récupérer le stock total de l'équipement
            var Clients = await _context.Clients
                .Include(r => r.Reservations)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<ClientDto>>(Clients);
        }
    }
}