using AceRental.Application.Packs.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Packs.Queries
{
    public class GetAllPacksHandle : IRequestHandler<GetAllPacksQuery, List<PackDetailsDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllPacksHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PackDetailsDto>> Handle(GetAllPacksQuery request, CancellationToken cancellationToken)
        {
            // 1. Récupérer le stock total de l'équipement
            var Packs = await _context.Packs
                .Include(x=> x.Items).ThenInclude(x=> x.Equipment)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<PackDetailsDto>>(Packs);
        }
    }
}