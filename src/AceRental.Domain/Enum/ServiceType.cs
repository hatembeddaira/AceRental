using System.ComponentModel;

namespace AceRental.Domain.Enum
{
    public enum ServiceType
    {
        [Description("Main-d’œuvre")]
        Labor = 0,
        
        [Description("Transport")]
        Transport = 1,
    }
}