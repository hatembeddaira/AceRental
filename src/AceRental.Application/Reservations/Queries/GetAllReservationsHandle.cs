using AceRental.Application.Reservations.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Reservations.Queries
{
    public class GetAllReservationsHandle : IRequestHandler<GetAllReservationsQuery, List<ReservationDetailsDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllReservationsHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ReservationDetailsDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
        {
            // 1. Récupérer le stock total de l'équipement
            var Reservations = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Invoices)
                .Include(r => r.Items).ThenInclude(i => i.Equipment)
                .Include(r => r.Items).ThenInclude(i => i.Pack)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<ReservationDetailsDto>>(Reservations);
        }
    }
}