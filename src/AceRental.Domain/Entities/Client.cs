using AceRental.Domain.Common;

namespace AceRental.Domain.Entities
{
    public class Client : BaseEntity
    {
        public required string ClientNumber { get; set; }
        public string? RaisonSociale { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? TelNumber { get; set; }
        public string? Address { get; set; }
        public string? ComplementAdresse { get; set; }
        public string? City { get; set; }
        public int PostalCode { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}