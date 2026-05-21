using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities
{
    public class Service : BaseEntity
    {
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public ServiceType Type { get; set; }
        public decimal PriceHT { get; set; }
        public bool IsDailyPrice { get; set; } = true; 
        public List<ReservationServices> Reservations { get; set; } = [];
    }
}