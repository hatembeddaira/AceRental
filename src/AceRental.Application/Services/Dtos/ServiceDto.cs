using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Domain.Enum;

namespace AceRental.Application.Services.Dtos
{
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public required string ServiceName { get; set; }
        public ServiceType Type { get; set; }
        public decimal DailyPriceHT { get; set; }
    }
}