using AceRental.Application.Services.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Services.Command;

public record CreateServiceCommand(
    string Name,
    string Reference,
    ServiceType Type,
    decimal PriceHT,
    bool IsDailyPrice
    ) : IRequest<ServiceDto>;