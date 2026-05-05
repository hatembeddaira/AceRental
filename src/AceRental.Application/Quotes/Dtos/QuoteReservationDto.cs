using AceRental.Application.Reservations.Dtos;

namespace AceRental.Application.Quotes.Dtos
{
    public class QuoteReservationDto
    {
        public Guid Id { get; set; }
        public required int QuoteNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TVA { get; set; } = 0.20m;
        public decimal TotalTTC => TotalHT * (1 + TVA);

        // Relation 1-to-1 avec la réservation
        public Guid ReservationId { get; set; }
    }
}