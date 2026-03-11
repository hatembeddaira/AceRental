using AceRental.Domain.Enum;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationHistoryDto
    {
        public Guid Id { get; set; }
        public Guid ReservationId { get; set; }
        public required HistoryType HistoryType {get; set;}
        public int VersionNumber { get; set; }
        public string? ChangeReason { get; set; }
        public required string DataSnapshotJson { get;  set; }
    }
}