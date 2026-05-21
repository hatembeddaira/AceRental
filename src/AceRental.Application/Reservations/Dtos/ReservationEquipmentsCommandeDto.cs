using System.ComponentModel.DataAnnotations;
using AceRental.Application.Equipments.Dtos;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationEquipmentsCommandeDto
    {
        public Guid EquipmentId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPriceAtTimeOfBooking { get; set; }
    }
}