using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Reservations.Queries
{
    public class GetAllReservationsHandle : IRequestHandler<GetAllReservationsQuery, IQueryable<ReservationDetailsDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllReservationsHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IQueryable<ReservationDetailsDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
        {
            
            return _context.Reservations
                .Include(r => r.Equipments)
                .Include(r => r.Packs)
                .Include(r => r.Services)
                .Include(r => r.Invoices)
                .Include(r => r.Quotes)
                .Include(r => r.Payments)
                .Include(r => r.Client)
                .AsNoTracking()
                .ProjectTo<ReservationDetailsDto>(_mapper.ConfigurationProvider);
        }
    }
}