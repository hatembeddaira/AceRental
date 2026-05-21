

using System.ComponentModel.DataAnnotations;
using AceRental.Application.Packs.Dtos;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationPacksCommandeDto
    {
        public Guid PackId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPriceAtTimeOfBooking { get; set; }
    }
}