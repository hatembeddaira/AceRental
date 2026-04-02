namespace AceRental.Application.Packs.Dtos
{
    public class PackDto
    {
        public Guid Id { get; set; }
        public required string Reference { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal DailyPriceHT { get; set; }
    }
}