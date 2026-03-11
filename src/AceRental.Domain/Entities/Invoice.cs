using AceRental.Domain.Common;

namespace AceRental.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public string InvoiceNumber { get; set; } = null!; // Ex: INV-2026-001
        public decimal AmountHT { get; set; }
        public decimal TaxRate { get; set; } = 20.0m;
        public bool IsPaid { get; set; }
        
        public Guid ReservationId { get; set; }
        public Reservation Reservation { get; set; } = null!;
    }
}