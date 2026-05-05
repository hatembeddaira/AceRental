using System.Text.Json.Serialization;
using AceRental.Application.Services.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Services.Command;

public record UpdateServiceCommand : IRequest<bool>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? ServiceName { get; set; }
    public ServiceType? Type { get; set; }
    public decimal? DailyPriceHT { get; set; }
}