
using AceRental.Domain.Enum;
using AceRental.Domain.Common;

namespace AceRental.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public required Guid ReservationId { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime Date { get; set; }
        public required PaymentMethod Method { get; set; } // Enum: Cash, Card, Transfer, etc.
        public required PaymentType  Type { get; set; }     // Enum: Deposit (Acompte), Installment (Tranche), Balance (Solde)
        public string? TransactionId { get; set; }     // Numéro de transaction ou chèque

        public virtual Reservation Reservation { get; set; } = null!;
    }
}