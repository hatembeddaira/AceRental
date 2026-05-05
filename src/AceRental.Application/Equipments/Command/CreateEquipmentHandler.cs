using AceRental.Application.Equipments.Dtos;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;

namespace AceRental.Application.Equipments.Command;

public class CreateEquipmentHandler : IRequestHandler<CreateEquipmentCommand, EquipmentDetailsDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateEquipmentHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EquipmentDetailsDto> Handle(CreateEquipmentCommand request, CancellationToken cancellationToken)
    {
        var equipment = new EquipmentDetailsDto
        {
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            Name = request.Name,
            Description = request.Description,
            DailyPriceHT = request.DailyPriceHT,
            PurchasePriceTTC = request.PurchasePriceTTC,
            NewPurchasePriceTTC = request.NewPurchasePriceTTC,
            TotalStock = request.TotalStock,
            Category = request.Category
        };
        
        _context.Equipments.Add(_mapper.Map<Equipment>(equipment));
        await _context.SaveChangesAsync(cancellationToken);
        return equipment;
    }
}

 
