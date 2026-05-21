using AceRental.Application.Reservations.Dtos;

namespace AceRental.Application.Quotes.Dtos
{
    public class QuoteDto
    {
        public Guid Id { get; set; }
        public int QuoteNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? ArchivedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public bool IsArchived { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TVA { get; set; } = 0.20m;
        public decimal TotalTTC => TotalHT * (1 + TVA);
        public Guid ReservationId { get; set; }
        public ReservationQuoteDto Reservation { get; set; } = null!;
        public ICollection<QuoteLinesDto> QuoteLines { get; set; } = [];
    }
}