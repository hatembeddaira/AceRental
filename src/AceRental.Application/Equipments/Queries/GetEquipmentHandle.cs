using AceRental.Application.Equipments.Dtos;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Equipments.Queries
{
    public class GetEquipmentHandle : IRequestHandler<GetEquipmentQuery, EquipmentDetailsDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEquipmentHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EquipmentDetailsDto> Handle(GetEquipmentQuery request, CancellationToken cancellationToken)
        {
            // 1. Récupérer le stock total de l'équipement
            var equipment = await _context.Equipments
                .AsNoTracking()
                .Include(e => e.PackItems).ThenInclude(pi => pi.Pack)
                .FirstOrDefaultAsync(e => e.Id == request.EquipmentId, cancellationToken);
            return _mapper.Map<EquipmentDetailsDto>(equipment);
        }
    }
}