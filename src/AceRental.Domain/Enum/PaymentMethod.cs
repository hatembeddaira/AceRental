using System.ComponentModel;

namespace AceRental.Domain.Enum
{
    public enum PaymentMethod
    {        
        [Description("Espèce")]
        Cash = 0,

        [Description("Carte")]
        Card = 1,

        [Description("Virement")]
        Transfer = 2,
        
        [Description("Lien de paiement")]
        PaymentLink = 3
    }
}