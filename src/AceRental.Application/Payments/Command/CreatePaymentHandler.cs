using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AceRental.Application.Payments.Dtos;
using AceRental.Application.Reservations.Queries;
using AceRental.Application.Payments.Queries;
using AceRental.Application.Reservations.Command;
using AutoMapper;
using AceRental.Domain.Enum;
using AceRental.Application.Exceptions;

namespace AceRental.Application.Payments.Command;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, PaymentDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public CreatePaymentHandler(ApplicationDbContext context, IMapper mapper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<PaymentDto> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);            
        if (reservation == null) 
            throw new NotFoundException(nameof(Reservation), request.ReservationId);

        if (request.Type == PaymentType.Installment && reservation.TotalTTC < 1500)
            throw new InvalidOperationException("Le paiement échelonné est réservé aux montants supérieurs à 1500€.");

        FinancialStatus financialStatus;
        if(request.Type == PaymentType.Balance)
        {   
            var montantPaiements = _context.Payments.Where(x=> x.ReservationId == request.ReservationId).Sum(p => p.Amount);
            if(reservation.TotalTTC != montantPaiements + request.Amount)
                throw new InvalidOperationException("Le type de paiement Balance doit correspondre au solde restant de la réservation.");
            
            financialStatus = FinancialStatus.Paid;   
        }
        else
        {
            financialStatus = FinancialStatus.PartiallyPaid;
        }

        var newPayment = new Payment
        {
            ReservationId = request.ReservationId,
            Amount = request.Amount,
            Date = request.Date,
            Method = request.Method,
            Type = request.Type,
            TransactionId = request.Method != PaymentMethod.Cash ? request.TransactionId : null,
        };
        
        // Ajouter le paiement à la collection
        _context.Payments.Add(newPayment);

        // Mettre à jour le statut financier global de la réservation
        var result = await _mediator.Send(new ChangeFinancialStatusCommand(){ ReservationId = request.ReservationId, Status = financialStatus }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<PaymentDto>(newPayment);
    }
}


