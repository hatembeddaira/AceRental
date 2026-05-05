using System.ComponentModel.DataAnnotations;
using AceRental.Application.Services.Dtos;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationServicesDto
    {
        public Guid ReservationId { get; set; }
        public Guid ServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPriceAtTimeOfBooking { get; set; } // Historisation du prix
        public ReservationDto Reservation { get; set; } = null!;
        public ServiceDto Service { get; set; } = null!;
    }
}