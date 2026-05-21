using AceRental.Application.Reservations.Dtos;

namespace AceRental.Application.Clients.Dtos
{
    public class ClientDto
    {
        public Guid Id { get; set; }
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
        public ICollection<ReservationDto> Reservations { get; set; } = new List<ReservationDto>();
    }
}