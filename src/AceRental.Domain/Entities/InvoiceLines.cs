using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities
{
    public class InvoiceLines : MinBaseEntity
    {
        public Guid InvoiceId { get; set; }
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public decimal DailyPriceHT { get; set; }
        public int Quantity { get; set; }
        public ReservationItemType Type { get; set; }
        public Invoice Invoice { get; set; } = null!;
    }
}