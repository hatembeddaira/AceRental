using AceRental.Application.Packs.Dtos;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AceRental.Application.Packs.Command;

public class CreatePackHandler : IRequestHandler<CreatePackCommand, Guid>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreatePackHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreatePackCommand request, CancellationToken cancellationToken)
    {
        if (request.Items.Count == 0) throw new Exception($"Pas d'équipements dans le pack.");
        // 1. Vérifier la disponibilité (Logique simplifiée)
        // Pour chaque item, on regarde si (StockTotal - DéjàAffecté) >= QuantitéDemandée
        foreach (var item in request.Items)
        {
            var isAvailable = await CheckAvailability(item.EquipmentId, item.Quantity);
            if (!isAvailable) throw new Exception($"Matériel ID {item.EquipmentId} quantité indisponible.");
        }

        var pack = new PackDetailsDto
        {            
            Id = Guid.NewGuid(),
            Reference = request.Reference,
            Name = request.Name,
            Description = request.Description,
            DailyPrice = request.DailyPrice
        };
        

        foreach (var item in request.Items)
        {
            pack.Items.Add(new PackItemDto
            {
                Id = Guid.NewGuid(),
                PackId = item.PackId,
                EquipmentId = item.EquipmentId,
                Quantity = item.Quantity
            });
        }

        _context.Packs.Add(_mapper.Map<Pack>(pack));
        await _context.SaveChangesAsync(cancellationToken);

        return pack.Id;
    }
    private async Task<bool> CheckAvailability(Guid equipmentId, int requestedQty)
    {
        // Logique : Somme des quantités louées dans les réservations qui chevauchent ces dates
        // var usedQty = await _context.PackItems
        //     .Where(ri => ri.EquipmentId == equipmentId)
        //     .SumAsync(ri => ri.Quantity);

        var totalStock = (await _context.Equipments.FindAsync(equipmentId))?.TotalStock ?? 0;

        // return (totalStock - usedQty) >= requestedQty;
        return totalStock >= requestedQty;
    }
}

 
