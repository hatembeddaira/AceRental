using AceRental.Domain.Enum;

namespace AceRental.Application.Equipments.Dtos
{
    public class EquipmentDto
    {
        public Guid Id {get; set;}
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal DailyPriceHT { get; set; }
        public decimal PurchasePriceTTC { get; set; }
        public decimal NewPurchasePriceTTC { get; set; }
        public int TotalStock { get; set; }
        public EquipmentCategory Category { get; set; }
    }
}