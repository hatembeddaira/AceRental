using AceRental.Application.Quotes.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Quotes.Queries
{
    public class GetAllQuotesHandle : IRequestHandler<GetAllQuotesQuery, IQueryable<QuoteDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllQuotesHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IQueryable<QuoteDto>> Handle(GetAllQuotesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Quotes
                .AsNoTracking()
                .Include(q => q.QuoteLines)
                .Include(q => q.Reservation)
                    .ThenInclude(r => r.Client)
                .Include(q => q.Reservation)
                   .ThenInclude(r => r.Payments);

            return query
                .IgnoreQueryFilters()
                .OrderByDescending(q => q.CreatedAt)
                .ProjectTo<QuoteDto>(_mapper.ConfigurationProvider);
        }
    }
}