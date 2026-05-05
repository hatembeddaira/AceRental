using AceRental.Application.Payments.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Payments.Queries
{
    public class GetAllPaymentsHandle : IRequestHandler<GetAllPaymentsQuery, IQueryable<PaymentDetailsDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllPaymentsHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IQueryable<PaymentDetailsDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            
            return _context.Payments
                .AsNoTracking()
                .ProjectTo<PaymentDetailsDto>(_mapper.ConfigurationProvider);
        }
    }
}