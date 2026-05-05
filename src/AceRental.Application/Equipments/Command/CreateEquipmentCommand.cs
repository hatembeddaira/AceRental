using System.ComponentModel.DataAnnotations;
using AceRental.Application.Equipments.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Equipments.Command;

public record CreateEquipmentCommand(
    [Required] string Reference,
    [Required] string Name,
    string Description,
    [Required] decimal DailyPriceHT,
    decimal PurchasePriceTTC,
    [Required] decimal NewPurchasePriceTTC,
    [Required] int TotalStock,
    EquipmentCategory Category 
    ) : IRequest<EquipmentDetailsDto>;
