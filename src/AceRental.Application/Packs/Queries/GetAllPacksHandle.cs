using AceRental.Application.Packs.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Packs.Queries
{
    public class GetAllPacksHandle : IRequestHandler<GetAllPacksQuery, IQueryable<PackDetailsDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllPacksHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IQueryable<PackDetailsDto>> Handle(GetAllPacksQuery request, CancellationToken cancellationToken)
        {
            return _context.Packs
                .Include(p => p.Items)
                    .ThenInclude(pi => pi.Equipment)
                .AsNoTracking()
                .ProjectTo<PackDetailsDto>(_mapper.ConfigurationProvider);
        }
    }
}