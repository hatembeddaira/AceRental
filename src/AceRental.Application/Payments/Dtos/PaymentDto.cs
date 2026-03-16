using AceRental.Domain.Enum;
using AceRental.Application.Reservations.Dtos;

namespace AceRental.Application.Payments.Dtos
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentType  Type { get; set; }
        public string? TransactionId { get; set; }

        public virtual ReservationDto Reservation { get; set; }       
    }
}