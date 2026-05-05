using System.ComponentModel.DataAnnotations.Schema;
using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities
{
    public class ReservationPacks : TraceEntity
    {
        public required Guid ReservationId { get; set; }
        public required Guid PackId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtTimeOfBooking { get; set; } // Historisation du prix
        public Reservation Reservation { get; set; } = null!;
        public Pack Pack { get; set; } = null!;
    }
}