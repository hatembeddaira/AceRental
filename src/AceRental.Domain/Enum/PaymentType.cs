using System.ComponentModel;

namespace AceRental.Domain.Enum
{
    public enum PaymentType
    {
        [Description("Acompte")]
        Deposit = 0,
        
        [Description("Tranche")]
        Installment = 1,

        [Description("Solde")]
        Balance = 2
    }
}