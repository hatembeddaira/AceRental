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
        public bool IsContentLocked => FinancialStatus == FinancialStatus.Invoiced || FinancialStatus == FinancialStatus.Paid;
        public decimal TotalAmount { get; set; }
        public Guid ClientId { get; set; }
        public required Client Client { get; set; }
        public int CurrentVersion { get; set; } = 1;
        public List<ReservationItem> Items { get; set; } = [];
        public List<ReservationHistory> History { get; set; } = [];

        // Règle : Pas de paiement si pas confirmé
        public bool CanMarkAsPaid()
        {
            return (Workflow == Workflow.B2C && LogisticStatus == LogisticStatus.Confirmed) || 
                   (Workflow == Workflow.B2B && FinancialStatus == FinancialStatus.Invoiced);
        }

        // Règle B2B : Pas de facture tant que le matériel n'est pas vérifié (Checked)
        public bool CanIssueInvoice()
        {
            return (Workflow == Workflow.B2B && LogisticStatus == LogisticStatus.Checked) || 
                   (Workflow == Workflow.B2C && FinancialStatus == FinancialStatus.Paid);
        }

        public bool CanChangeWorkflow()
        {
            return (Workflow == Workflow.B2C && LogisticStatus == LogisticStatus.Confirmed) || 
                   (Workflow == Workflow.B2B && LogisticStatus == LogisticStatus.Confirmed);
        }

        public bool CanMarkAsFinished()
        {
            return (Workflow == Workflow.B2C && LogisticStatus == LogisticStatus.Checked) || 
                   (Workflow == Workflow.B2B && FinancialStatus == FinancialStatus.Paid);
        }
    }
}