using AceRental.Application.Equipments.Dtos;
using AceRental.Application.Packs.Dtos;
using AceRental.Domain.Entities;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationItemDto
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }

        // Soit on loue un équipement seul
        public Guid? EquipmentId { get; set; }
        public EquipmentDto? Equipment { get; set; }

        // Soit on loue un pack complet
        public Guid? PackId { get; set; }
        public PackDto? Pack { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtTimeOfBooking { get; set; } // Historisation du prix
        // public Reservation Reservation { get; set; } = null!;
    }
}
