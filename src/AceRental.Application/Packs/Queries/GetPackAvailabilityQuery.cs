using MediatR;

namespace AceRental.Application.Packs.Queries;

public record GetPackAvailabilityQuery(
    Guid PackId, 
    DateTime StartDate, 
    DateTime EndDate
) : IRequest<int>;