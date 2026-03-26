using AceRental.Application.Reservations.Dtos;

namespace AceRental.Application.Clients.Dtos
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public required string ClientNumber { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        
        public ICollection<ReservationDetailsDto> Reservations { get; set; } = new List<ReservationDetailsDto>();
    }
}