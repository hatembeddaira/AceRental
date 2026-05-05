using System.Text.Json.Serialization;
using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Reservations.Command;

public record UpdateReservationCommand() : IRequest<bool>
{
    [JsonIgnore]
    public Guid ReservationId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public List<ReservationEquipmentsDto>? Equipments { get; set; }
    public List<ReservationPacksDto>? Packs { get; set; }
    public List<ReservationServicesDto>? Services { get; set; }
}