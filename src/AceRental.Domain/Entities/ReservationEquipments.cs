using System.ComponentModel.DataAnnotations.Schema;
using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities
{
    public class ReservationEquipments : TraceEntity
    {
        public required Guid ReservationId { get; set; }
        public required Guid EquipmentId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtTimeOfBooking { get; set; } // Historisation du prix
        public Reservation Reservation { get; set; } = null!;
        public Equipment Equipment { get; set; } = null!;
    }
}