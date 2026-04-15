using AceRental.Domain.Entities;
using AceRental.Application.Exceptions;
using AceRental.Application.Reservations.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Reservations.Queries
{
    public class GetReservationHandle : IRequestHandler<GetReservationQuery, ReservationDetailsDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetReservationHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReservationDetailsDto> Handle(GetReservationQuery request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Invoices)
                .Include(r => r.Items).ThenInclude(i => i.Equipment)
                .Include(r => r.Items).ThenInclude(i => i.Pack)            
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.ReservationId, cancellationToken);         
            return _mapper.Map<ReservationDetailsDto>(reservation);
        }
    }
}