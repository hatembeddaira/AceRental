using System.ComponentModel;
using AceRental.Domain.Common;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Entities
{

    public class Reservation : BaseEntity
    {
        public required int ReservationNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required FinancialStatus FinancialStatus { get; set; }
        public required LogisticStatus LogisticStatus { get; set; }
        public required Workflow Workflow { get; set; }
        public decimal TotalHT { get; set; }
        public decimal TVA { get; set; } 
        public decimal TotalTTC  { get; set; }
        public Guid ClientId { get; set; }
        public required Client Client { get; set; }
        public int CurrentVersion { get; set; } = 1;
        public List<ReservationItem> Items { get; set; } = [];
        public List<ReservationHistory> History { get; set; } = [];
        public List<Invoice> Invoices { get; set; } = [];

        
    }
}