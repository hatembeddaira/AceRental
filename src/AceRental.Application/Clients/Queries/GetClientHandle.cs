using AceRental.Application.Clients.Dtos;
using AceRental.Application.Clients.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Clients.Queries
{
    public class GetClientHandle : IRequestHandler<GetClientQuery, ClientDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetClientHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            // 1. Récupérer le stock total de l'équipement
            var Client = await _context.Clients
                .Include(r => r.Reservations)          
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.ClientId, cancellationToken);
            return _mapper.Map<ClientDto>(Client);
        }
    }
}