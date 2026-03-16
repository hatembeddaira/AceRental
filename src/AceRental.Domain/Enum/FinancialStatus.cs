using System.ComponentModel;

namespace AceRental.Domain.Enum
{
    public enum FinancialStatus
    {
        Unpaid = 0, 
        PartiallyPaid = 1,
        Paid = 2, 
        Invoiced =  3, 
        Refunded = 4
        
    }
}