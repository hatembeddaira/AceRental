using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Equipments.Command;

public record CreateEquipmentCommand(
    string Reference,
    string Name,
    string Description,
    decimal DailyPrice,
    decimal PurchasePrice,
    decimal NewPurchasePrice,
    int TotalStock,
    EquipmentCategory Category
    ) : IRequest<Guid>;
