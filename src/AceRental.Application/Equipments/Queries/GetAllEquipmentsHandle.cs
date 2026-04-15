using AceRental.Application.Equipments.Dtos;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Equipments.Queries
{
    public class GetAllEquipmentsHandle : IRequestHandler<GetAllEquipmentsQuery, IQueryable<EquipmentDetailsDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetAllEquipmentsHandle(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IQueryable<EquipmentDetailsDto>> Handle(GetAllEquipmentsQuery request, CancellationToken cancellationToken)
        {
            return _context.Equipments
                .AsNoTracking()
                .ProjectTo<EquipmentDetailsDto>(_mapper.ConfigurationProvider);
        }
    }
}