using AceRental.Application.Invoices.Dtos;
using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Application.Invoices.Dtos
{
    public class InvoiceLinesDto
    {
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public decimal DailyPriceHT { get; set; }
        public int Quantity { get; set; }
        public ReservationItemType Type { get; set; }
        public InvoiceDto Invoice { get; set; } = null!;
    }
}