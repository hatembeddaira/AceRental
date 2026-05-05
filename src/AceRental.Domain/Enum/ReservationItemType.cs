using System.ComponentModel;

namespace AceRental.Domain.Enum
{
    public enum ReservationItemType
    {
        [Description("Équipement")]
        Equipment = 0,
        
        [Description("Pack")]
        Pack = 1,

        [Description("Service")]
        Service = 2
        
    }
}