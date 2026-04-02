using AceRental.Domain.Common;

namespace AceRental.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public int InvoiceNumber { get; set; } 
        public decimal AmountHT { get; set; }
        public decimal TaxRate { get; set; } = 20.0m;
        public bool IsPaid { get; set; }
        
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;
    }
}