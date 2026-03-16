using AceRental.Application.Payments.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Payments.Queries
{
    public class GetAllPaymentsHandle : IRequestHandler<GetAllPaymentsQuery, List<PaymentDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllPaymentsHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<PaymentDto>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
        {
            
            var payments = await _context.Payments
                .Include(r => r.Reservation)           
                .AsNoTracking()
                .Where(e => e.ReservationId == request.ReservationId)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<PaymentDto>>(payments);
        }
    }
}