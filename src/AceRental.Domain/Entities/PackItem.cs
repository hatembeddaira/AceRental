using AceRental.Domain.Common;

namespace AceRental.Domain.Entities
{
    public class PackItem : BaseEntity
    {
        public required Guid PackId { get; set; }
        public Pack Pack { get; set; } = null!;
        public required Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; }  = null!;
        public required int Quantity { get; set; }
    }
}