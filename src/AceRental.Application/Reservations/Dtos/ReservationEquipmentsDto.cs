using System.ComponentModel.DataAnnotations;
using AceRental.Application.Equipments.Dtos;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationEquipmentsDto
    {
        public Guid ReservationId { get; set; }
        public Guid EquipmentId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtTimeOfBooking { get; set; } // Historisation du prix
        public ReservationDto Reservation { get; set; } = null!;
        public EquipmentDto Equipment { get; set; } = null!;
    }
}