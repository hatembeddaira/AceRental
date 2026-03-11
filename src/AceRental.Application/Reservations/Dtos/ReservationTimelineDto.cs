using AceRental.Domain.Enum;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationTimelineDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public HistoryType HistoryType { get; set; }
        public required string Title { get; set; }
        public required string User { get; set; }
        public object? Details { get; set; } // Les données JSON désérialisées
    }
}