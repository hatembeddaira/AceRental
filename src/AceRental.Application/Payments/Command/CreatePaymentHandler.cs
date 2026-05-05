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

        var resevations = await _mediator.Send(new GetAllReservationsQuery(), cancellationToken);
        if (resevations == null) 
            throw new KeyNotFoundException("Pas de réservation");

        var resultResevation = resevations.Where(r => r.Id == request.ReservationId);

        if (resultResevation == null) 
            throw new KeyNotFoundException("Réservation introuvable.");
        if (request.Type == PaymentType.Installment && resultResevation.Select(r => r.TotalTTC).FirstOrDefault() < 1500)
            throw new InvalidOperationException("Le paiement échelonné est réservé aux montants supérieurs à 1500€.");

        FinancialStatus financialStatus;
        if(request.Type == PaymentType.Balance)
        {   

            var resultPayments = await _mediator.Send(new GetAllPaymentsQuery(), cancellationToken);
            if(resultResevation.Select(r => r.TotalTTC).FirstOrDefault() != 
                resultPayments.Where(x=> x.ReservationId == request.ReservationId).Sum(p => p.Amount) + request.Amount)
                throw new InvalidOperationException("Le type de paiement Balance doit correspondre au solde restant de la réservation.");
            
            financialStatus = FinancialStatus.Paid;   
        }
        else
        {
            financialStatus = FinancialStatus.PartiallyPaid;
        }

        var newPayment = new PaymentDto
        {
            Id = Guid.NewGuid(),
            ReservationId = request.ReservationId,
            Amount = request.Amount,
            Date = request.Date,
            Method = request.Method,
            Type = request.Type,
            TransactionId = request.Method != PaymentMethod.Cash ? request.TransactionId : null,
        };
        
        // Ajouter le paiement à la collection
        _context.Payments.Add(_mapper.Map<Payment>(newPayment));

        // Mettre à jour le statut financier global de la réservation
        var result = await _mediator.Send(new ChangeFinancialStatusCommand(){ ReservationId = request.ReservationId, Status = financialStatus }, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return newPayment;
    }
}


