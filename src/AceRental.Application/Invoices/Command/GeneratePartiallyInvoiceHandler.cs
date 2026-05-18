using MediatR;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AceRental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Application.Exceptions;
using AceRental.Domain.Extensions;
using FluentValidation.Results;

namespace AceRental.Application.Invoices.Command
{
    public class GeneratePartiallyInvoiceHandler : IRequestHandler<GeneratePartiallyInvoiceCommand, Guid>
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GeneratePartiallyInvoiceHandler(ApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Guid> Handle(GeneratePartiallyInvoiceCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);            
            if (reservation == null) 
                throw new NotFoundException(nameof(Reservation), request.ReservationId);
            if(request.AmountHT <= 0)
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.AmountHT), "Le montant doit être supérieur à zéro.")
                });
            if(request.AmountHT >= reservation.TotalHT)
                throw new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure(nameof(request.AmountHT), "Le montant doit être inférieur au total de la réservation.")
                });
            
            if (!reservation.FinancialStatus.CanTransitionTo(FinancialStatus.PartiallyInvoiced, reservation))
                throw new BusinessRuleException($"Transition impossible de {reservation?.FinancialStatus} vers {FinancialStatus.PartiallyInvoiced} " +
                $"dans le workflow {reservation!.Workflow} avec un statut logistique = {reservation!.LogisticStatus}");

            var partiallyInvoice = new Invoice
            {
                ReservationId = reservation.Id,
                AmountHT = request.AmountHT,
                Type = InvoiceType.PartiallyInvoice
            };

            reservation.FinancialStatus = FinancialStatus.PartiallyInvoiced;

            _context.Invoices.Add(partiallyInvoice);
            await _context.SaveChangesAsync(cancellationToken);
            // Send Mail or Notification 
            return partiallyInvoice.Id;
        }
    }
}