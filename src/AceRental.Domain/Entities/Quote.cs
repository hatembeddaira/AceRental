using AceRental.Domain.Common;

namespace AceRental.Domain.Entities;
public class Quote : BaseEntity
{
    public required int QuoteNumber { get; set; }
    public DateTime ExpiryDate { get; set; } // Validité du devis (ex: +15 jours)
    public decimal TotalHT { get; set; }
    public decimal TVA { get; set; } 
    public decimal TotalTTC  { get; set; }

    // Relation 1-to-1 avec la réservation
    public Guid ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
}