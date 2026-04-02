using System.ComponentModel;

namespace AceRental.Domain.Enum
{
    public enum LogisticStatus
    {
        [Description("Brouillon")]
        Draft = 0,

        [Description("Supprimé")]
        Deleted = 1,

        [Description("Devis")]
        Quote = 2,

        [Description("Panier")]
        Basket = 3,

        [Description("Annulée")]
        Cancelled = 4,

        [Description("Confirmée/Réservée")]
        Confirmed = 5,

        [Description("Matériel sorti")]
        PickedUp = 6,

        [Description("Matériel retourné")]
        Returned = 7,

        [Description("Matériel vérifier")]
        Checked = 8,

        [Description("Matériel endommagé")]
        Damaged = 9,
        
        [Description("Terminé")]
        Finished = 10,
    }
}