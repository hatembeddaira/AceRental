using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public int InvoiceNumber { get; set; } 
        public decimal AmountHT { get; set; }
        public decimal TaxRate { get; set; } = 0.20m;
        public bool IsPaid { get; set; }        
        public InvoiceType Type { get; set; }
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;
        public List<Payment> Payments { get; set; } = [];
    }
}