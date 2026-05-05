using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities
{
    public class Service : BaseEntity
    {
        public required string ServiceName { get; set; }
        public ServiceType Type { get; set; }
        public decimal DailyPriceHT { get; set; }
        public List<ReservationServices> Reservations { get; set; } = [];
    }
}