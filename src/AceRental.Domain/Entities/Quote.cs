using AceRental.Domain.Common;

namespace AceRental.Domain.Entities;
public class Quote : ArchivedEntity
{
    public int QuoteNumber { get; set; }
    public DateTime ExpiryDate { get; set; } // Validité du devis (ex: +15 jours)
    public decimal TotalHT { get; set; }
    public decimal TVA { get; set; }  = 0.20m;
    public Guid ReservationId { get; set; }
    public Reservation Reservation { get; set; } = null!;
    public ICollection<QuoteLines> QuoteLines { get; set; } = [];
}