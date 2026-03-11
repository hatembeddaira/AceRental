using AceRental.Application.Packs.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Packs.Queries
{
    public class GetPackHandle : IRequestHandler<GetPackQuery, PackDetailsDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPackHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PackDetailsDto> Handle(GetPackQuery request, CancellationToken cancellationToken)
        {
            // 1. Récupérer le stock total de l'équipement
            var Pack = await _context.Packs
                .Include(x=> x.Items).ThenInclude(x=> x.Equipment)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.PackId, cancellationToken);
            return _mapper.Map<PackDetailsDto>(Pack);
        }
    }
}