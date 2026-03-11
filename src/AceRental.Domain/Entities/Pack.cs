using AceRental.Domain.Common;

namespace AceRental.Domain.Entities
{
    public class Pack : BaseEntity
    {
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal DailyPrice { get; set; }
        public ICollection<PackItem> Items { get; set; } = new List<PackItem>();
    }
}