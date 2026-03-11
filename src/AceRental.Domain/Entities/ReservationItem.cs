using AceRental.Domain.Common;

namespace AceRental.Domain.Entities
{
    public class ReservationItem : BaseEntity
    {
        public Guid ReservationId { get; set; }
        
        // Soit on loue un équipement seul
        public Guid? EquipmentId { get; set; }
        public Equipment? Equipment { get; set; }
        
        // Soit on loue un pack complet
        public Guid? PackId { get; set; }
        public Pack? Pack { get; set; }        
        public int Quantity { get; set; }
        public decimal UnitPriceAtTimeOfBooking { get; set; } // Historisation du prix
        public Reservation Reservation { get; set; } = null!;
    }
}