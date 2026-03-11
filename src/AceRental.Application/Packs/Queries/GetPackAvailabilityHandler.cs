using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Infrastructure.Persistence;
using AceRental.Domain.Enum;

namespace AceRental.Application.Packs.Queries;

public class GetPackAvailabilityHandler : IRequestHandler<GetPackAvailabilityQuery, int>
{
    private readonly ApplicationDbContext _context;

    public GetPackAvailabilityHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(GetPackAvailabilityQuery request, CancellationToken cancellationToken)
    {
        // 1. Récupérer la composition du pack
        var packItems = await _context.PackItems
            .Include(pi => pi.Equipment)
            .Where(pi => pi.PackId == request.PackId)
            .ToListAsync(cancellationToken);

        if (!packItems.Any()) return 0;

        int maxPacksPossible = int.MaxValue;

        // 2. Pour chaque équipement dans le pack
        foreach (var item in packItems)
        {
            // Calculer combien il en reste en stock (même logique que pour un produit seul)
            var rentedQty = await _context.ReservationItems
                .Where(ri => ri.EquipmentId == item.EquipmentId &&
                             ri.Reservation.LogisticStatus != LogisticStatus.Cancelled &&
                             ri.Reservation.StartDate < request.EndDate && 
                             ri.Reservation.EndDate > request.StartDate)
                .SumAsync(ri => ri.Quantity, cancellationToken);

            var availableQty = item.Equipment.TotalStock - rentedQty;

            // 3. Calculer combien de packs on peut faire avec cet équipement précis
            // Ex: Il reste 5 enceintes, le pack en demande 2 -> On peut faire 2 packs (5/2 = 2.5)
            var packsPossibleWithThisItem = availableQty / item.Quantity;

            if (packsPossibleWithThisItem < maxPacksPossible)
            {
                maxPacksPossible = packsPossibleWithThisItem;
            }
        }

        return maxPacksPossible < 0 ? 0 : maxPacksPossible;
    }
}