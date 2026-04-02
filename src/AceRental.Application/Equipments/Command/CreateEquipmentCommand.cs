using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Equipments.Command;

public record CreateEquipmentCommand(
    string Reference,
    string Name,
    string Description,
    decimal DailyPriceHT,
    decimal PurchasePriceTTC,
    decimal NewPurchasePriceTTC,
    int TotalStock,
    EquipmentCategory Category
    ) : IRequest<Guid>;
