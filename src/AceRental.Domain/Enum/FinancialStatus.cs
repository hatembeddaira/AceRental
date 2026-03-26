using System.ComponentModel;

namespace AceRental.Domain.Enum
{
    public enum FinancialStatus
    {
        Unpaid = 0, 
        PartiallyPaid = 1,
        Paid = 2, 
        PartiallyInvoiced =  3,
        RepairRentalInvoiced =  4,
        RentalInvoiced =  5, 
        Refunded = 6
    }
}