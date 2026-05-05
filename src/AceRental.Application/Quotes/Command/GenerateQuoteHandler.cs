using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AceRental.Domain.Enum;
using AceRental.Application.Exceptions;
using AceRental.Application.Quotes.Dtos;

namespace AceRental.Application.Quotes.Command;

public class GenerateQuoteHandler : IRequestHandler<GenerateQuoteCommand, Guid>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GenerateQuoteHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(GenerateQuoteCommand request, CancellationToken cancellationToken)
    {
        // 1. Récupérer la réservation 
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);

        if (reservation == null) 
            throw new NotFoundException(nameof(Reservation), request.ReservationId);

        // 2. Créer l'objet Devis
        var quote = new QuoteDto
        {
            Id = Guid.NewGuid(),
            ReservationId = reservation.Id,
            QuoteNumber = await GenerateQuoteNumber(), // Logique de numérotation à améliorer
            CreatedAt = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(15),
            TotalHT = reservation.TotalHT
        };

        // 3. Mettre à jour le statut de la réservation si nécessaire
        reservation.LogisticStatus = LogisticStatus.Quote;

        _context.Quotes.Add(_mapper.Map<Quote>(quote));
        await _context.SaveChangesAsync(cancellationToken);
        // Send Mail or Notification 
        return quote.Id;
    }
    private async Task<int> GenerateQuoteNumber()
    {
        var lastReservationNumber = await _context.Quotes
            .IgnoreQueryFilters()
            .Where(ri => ri.CreatedAt.Year == DateTime.Now.Year)
            .OrderByDescending(ri => ri.QuoteNumber)
            .Select(x=> x.QuoteNumber).FirstOrDefaultAsync();

        if(lastReservationNumber == 0)
            lastReservationNumber = DateTime.Now.Year * 1000;
            
        return lastReservationNumber + 1;
    }
}