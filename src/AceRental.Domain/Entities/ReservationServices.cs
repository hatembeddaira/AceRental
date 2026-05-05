using System.ComponentModel.DataAnnotations.Schema;
using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities
{
    public class ReservationServices : TraceEntity
    {
        public required Guid ReservationId { get; set; }
        public required Guid ServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtTimeOfBooking { get; set; } // Historisation du prix
        public Reservation Reservation { get; set; } = null!;
        public Service Service { get; set; } = null!;
    }
}