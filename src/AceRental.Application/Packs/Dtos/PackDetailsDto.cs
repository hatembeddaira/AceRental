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
    }
}