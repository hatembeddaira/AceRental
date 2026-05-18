using System.Text.Json.Serialization;
using AceRental.Application.Services.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Services.Command;

public record UpdateServiceCommand : IRequest<bool>
{
    [JsonIgnore]
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Reference { get; set; }
    public ServiceType? Type { get; set; }
    public decimal? DailyPriceHT { get; set; }
}