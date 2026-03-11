using AceRental.Application.Equipments.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Equipments.Queries
{
    public class GetAllEquipmentsHandle : IRequestHandler<GetAllEquipmentsQuery, List<EquipmentDetailsDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllEquipmentsHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<EquipmentDetailsDto>> Handle(GetAllEquipmentsQuery request, CancellationToken cancellationToken)
        {
            // 1. Récupérer le stock total de l'équipement
            var equipments = await _context.Equipments
                .AsNoTracking()
                .Include(e => e.PackItems).ThenInclude(pi => pi.Pack)
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<EquipmentDetailsDto>>(equipments);
        }
    }
}