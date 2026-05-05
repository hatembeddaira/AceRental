using AceRental.Application.Services.Dtos;
using AceRental.Domain.Enum;
using MediatR;

namespace AceRental.Application.Services.Command;

public record CreateServiceCommand(
    string ServiceName,
    ServiceType Type,
    decimal DailyPriceHT
    ) : IRequest<ServiceDto>;