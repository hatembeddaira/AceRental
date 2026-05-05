using System.Text.Json.Serialization;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Reservations.Command
{
    public class ChangeFinancialStatusCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid ReservationId { get; set; }
        public FinancialStatus Status { get; set; }
    }
}