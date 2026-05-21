using System.ComponentModel.DataAnnotations;
using AceRental.Application.Services.Dtos;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationServicesCommandeDto
    {
        public Guid ServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPriceAtTimeOfBooking { get; set; }
    }
}