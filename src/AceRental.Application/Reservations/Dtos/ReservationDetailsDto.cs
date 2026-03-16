using AceRental.Application.Clients.Dtos;
using AceRental.Application.Payments.Dtos;
using AceRental.Domain.Enum;

namespace AceRental.Application.Reservations.Dtos
{
    public class ReservationDetailsDto
    {
        public Guid Id { get; set; }
        public required int ReservationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required FinancialStatus FinancialStatus { get; set; }
        public required LogisticStatus LogisticStatus { get; set; }
        public required Workflow Workflow { get; set; }
        public bool IsContentLocked => FinancialStatus == FinancialStatus.Invoiced || FinancialStatus == FinancialStatus.Paid;
        public decimal TotalAmount { get; set; }        
        public Guid ClientId { get; set; }
        public ClientReservationDto Client { get; set; } = null!;
        public int CurrentVersion { get; set; } = 1;
        public ICollection<ReservationItemDto> Items { get; set; } = [];
        public ICollection<PaymentDto> Payments { get; set; } = [];
    }
}