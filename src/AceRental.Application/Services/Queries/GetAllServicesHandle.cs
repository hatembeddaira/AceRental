using AceRental.Application.Services.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Services.Queries
{
    public class GetAllServicesHandle : IRequestHandler<GetAllServicesQuery, IQueryable<ServiceDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllServicesHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IQueryable<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            return _context.Services
                .AsNoTracking()
                .OrderBy(s => s.ServiceName)
                .ProjectTo<ServiceDto>(_mapper.ConfigurationProvider);
        }
    }
}