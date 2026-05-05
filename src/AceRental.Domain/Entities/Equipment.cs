using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities;

public class Equipment : BaseEntity
{
    public required string Reference { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal DailyPriceHT { get; set; }
    public decimal PurchasePriceTTC { get; set; }
    public decimal NewPurchasePriceTTC { get; set; }
    public int TotalStock { get; set; }
    public EquipmentCategory Category { get; set; } = EquipmentCategory.Autres;
    public ICollection<PackItem> PackItems { get; set; } = [];
    public List<ReservationEquipments> Reservations { get; set; } = [];
}