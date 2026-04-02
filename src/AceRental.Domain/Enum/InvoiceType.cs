using System.ComponentModel;

namespace AceRental.Domain.Enum
{
    public enum InvoiceType
    {

        [Description("Facture de location standard")]
        RentalInvoice = 0,

        [Description("Facture de location et de remise en état (Damage)")]
        RepairRentalInvoice = 1,

        [Description("Facture d'acompte")]
        PartiallyInvoice = 2,

        [Description("Facture de remboursement")]
        RefundInvoice = 3
    }
}