using AceRental.Domain.Enum;

namespace AceRental.Application.Services.Dtos
{
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public ServiceType Type { get; set; }
        public decimal DailyPriceHT { get; set; }
    }
}