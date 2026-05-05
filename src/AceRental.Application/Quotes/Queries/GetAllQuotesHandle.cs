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
            return _context.Quotes
                .AsNoTracking()
                .Include(q => q.Reservation)
                    .ThenInclude(r => r.Client)           // Client de la réservation
                //.Include(q => q.Reservation)
                //    .ThenInclude(r => r.Items)            // Items de la réservation
                //.Include(q => q.Reservation)
                //    .ThenInclude(r => r.Invoices)         // Factures de la réservation
                //        .ThenInclude(i => i.Payments)     // Paiements des factures
                .Include(q => q.Reservation)
                   .ThenInclude(r => r.Payments)         // Paiements directs de la réservation
                .OrderByDescending(q => q.CreatedAt)
                .ProjectTo<QuoteDto>(_mapper.ConfigurationProvider);
        }
    }
}