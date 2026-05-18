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

public class GenerateQuoteHandler : IRequestHandler<GenerateQuoteCommand, QuoteDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GenerateQuoteHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<QuoteDto> Handle(GenerateQuoteCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Equipments)
                .ThenInclude(re => re.Equipment)
            .Include(r => r.Packs)
                .ThenInclude(rp => rp.Pack)
            .Include(r => r.Services)
                .ThenInclude(rs => rs.Service)
            .Include(r => r.Client)
            .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);

        if (reservation == null) 
            throw new NotFoundException(nameof(Reservation), request.ReservationId);
        
        if (!reservation.LogisticStatus.CanTransitionTo(LogisticStatus.Quote, reservation))
                throw new BusinessRuleException($"Transition impossible de {reservation?.LogisticStatus} vers {LogisticStatus.Quote} " +
                $"dans le workflow {reservation!.Workflow} avec un statut financière = {reservation!.FinancialStatus}");

        List<QuoteLines> quoteLines = [];

        quoteLines.AddRange(reservation.Equipments.Select(rl => new QuoteLines
            {
                Reference = rl.Equipment.Reference,
                Name = rl.Equipment.Name,
                DailyPriceHT = rl.UnitPriceAtTimeOfBooking,
                Quantity = rl.Quantity,
                Type = ReservationItemType.Equipment
            }).ToList());
        quoteLines.AddRange(reservation.Packs.Select(rl => new QuoteLines
            {
                Reference = rl.Pack.Reference,
                Name = rl.Pack.Name,
                DailyPriceHT = rl.UnitPriceAtTimeOfBooking,
                Quantity = rl.Quantity,
                Type = ReservationItemType.Pack
            }).ToList());
        quoteLines.AddRange(reservation.Services.Select(rl => new QuoteLines
            {
                Reference = rl.Service.Reference,
                Name = rl.Service.Name,
                DailyPriceHT = rl.UnitPriceAtTimeOfBooking,
                Quantity = rl.Quantity,
                Type = ReservationItemType.Service
            }).ToList());


        var quote = new Quote
        {
            ReservationId = reservation.Id,
            TotalHT = reservation.TotalHT,
            QuoteLines = quoteLines,
        };

        reservation.LogisticStatus = LogisticStatus.Quote;

        _context.Quotes.Add(quote);
        await _context.SaveChangesAsync(cancellationToken);
        // Send Mail or Notification 
        return _mapper.Map<QuoteDto>(quote);
    }
}