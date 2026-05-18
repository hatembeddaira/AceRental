using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Domain.Enum;

namespace AceRental.Application.Quotes.Dtos
{
    public class QuoteLinesDto
    {
        public Guid Id { get; set; }
        public Guid QuoteId { get; set; }
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public decimal DailyPriceHT { get; set; }
        public int Quantity { get; set; }
        public ReservationItemType Type { get; set; }
        public QuoteDto Quote { get; set; } = null!;
    }
}