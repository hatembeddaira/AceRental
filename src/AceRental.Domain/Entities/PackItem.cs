using AceRental.Domain.Common;

namespace AceRental.Domain.Entities
{
    public class PackItem : BaseEntity
    {
        public Guid PackId { get; set; }
        public required Pack Pack { get; set; }
        public required Guid EquipmentId { get; set; }
        public required Equipment Equipment { get; set; } 
        public required int Quantity { get; set; }
    }
}