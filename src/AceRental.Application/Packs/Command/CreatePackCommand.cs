using AceRental.Application.Packs.Dtos;
using MediatR;

namespace AceRental.Application.Packs.Command;

public record CreatePackCommand(
    string Reference,
    string Name,
    string Description,
    decimal DailyPrice,
    List<PackItemDto> Items
    ) : IRequest<Guid>;
