using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Services.Dtos;

namespace AceRental.Application.Packs.Dtos
{
    public class PackDetailsDto
    {
        public Guid Id { get; set; }
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal DailyPriceHT { get; set; }
        public ICollection<PackItemDto> Items { get; set; } = new List<PackItemDto>();
        public ICollection<PackServiceDto> Services { get; set; } = new List<PackServiceDto>();
    }
}