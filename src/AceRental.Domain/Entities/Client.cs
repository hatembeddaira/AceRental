using AceRental.Domain.Common;

namespace AceRental.Domain.Entities
{
    public class Client : BaseEntity
    {
        public required string ClientNumber { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}