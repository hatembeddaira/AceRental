using AceRental.Application.Clients.Dtos;
using AceRental.Application.Payments.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Payments.Queries
{
    public class GetPaymentDetailsHandle : IRequestHandler<GetPaymentDetailsQuery, PaymentDetailsDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPaymentDetailsHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaymentDetailsDto> Handle(GetPaymentDetailsQuery request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments
                .Include(r => r.Reservation)           
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.PaymentId, cancellationToken);
            return _mapper.Map<PaymentDetailsDto>(payment);
        }
    }
}