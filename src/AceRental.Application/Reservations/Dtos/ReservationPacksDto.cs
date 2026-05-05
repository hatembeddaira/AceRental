

using System.ComponentModel.DataAnnotations;
using AceRental.Application.Packs.Dtos;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationPacksDto
    {
        public Guid ReservationId { get; set; }
        public Guid PackId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtTimeOfBooking { get; set; } // Historisation du prix
        public ReservationDto Reservation { get; set; } = null!;
        public PackDto Pack { get; set; } = null!;
    }
}