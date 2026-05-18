using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AceRental.Domain.Enum;
using AceRental.Application.Exceptions;
using AceRental.Application.Quotes.Dtos;
using AceRental.Domain.Extensions;

namespace AceRental.Application.Quotes.Command;

public class CancelQuoteHandler : IRequestHandler<CancelQuoteCommand, bool>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CancelQuoteHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<bool> Handle(CancelQuoteCommand request, CancellationToken cancellationToken)
    {
        var quote = await _context.Quotes
            .Include(q => q.Reservation)
            .FirstOrDefaultAsync(r => r.Id == request.QuoteId, cancellationToken);

        if (quote == null) 
            throw new NotFoundException(nameof(Quote), request.QuoteId);
        
        if (!quote.Reservation.LogisticStatus.CanTransitionTo(LogisticStatus.Cancelled, quote.Reservation))
                throw new BusinessRuleException($"Transition impossible de {quote.Reservation?.LogisticStatus} vers {LogisticStatus.Cancelled} " +
                $"dans le workflow {quote.Reservation!.Workflow} avec un statut financière = {quote.Reservation!.FinancialStatus}");

        // Archivage du devis
        _context.Quotes.Remove(quote);
        await _context.SaveChangesAsync(cancellationToken);
        // Send Mail or Notification 
        return true;
    }
}