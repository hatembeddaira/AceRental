
using AceRental.Application.Exceptions;
using AceRental.Domain.Entities;
using AceRental.Domain.Enum;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Extensions;

namespace AceRental.Application.Invoices.Command
{
    public class GenerateRentalInvoiceHandler : IRequestHandler<GenerateRentalInvoiceCommand, Guid>
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GenerateRentalInvoiceHandler(ApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<Guid> Handle(GenerateRentalInvoiceCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);            
            if (reservation == null) 
                throw new NotFoundException(nameof(Reservation), request.ReservationId);
            
            if (!reservation.FinancialStatus.CanTransitionTo(FinancialStatus.RentalInvoiced, reservation))
                throw new BusinessRuleException($"Transition impossible de {reservation?.FinancialStatus} vers {FinancialStatus.RentalInvoiced} " +
                $"dans le workflow {reservation!.Workflow} avec un statut logistique = {reservation!.LogisticStatus}");

            var rentalInvoice = new Invoice
            {
                ReservationId = reservation.Id,
                AmountHT = reservation.TotalHT,
                IsPaid = true,
                Type = InvoiceType.RentalInvoice
            };

            reservation.FinancialStatus = FinancialStatus.RentalInvoiced;

            _context.Invoices.Add(rentalInvoice);
            await _context.SaveChangesAsync(cancellationToken);
            // Send Mail or Notification 
            return rentalInvoice.Id;
        }
    }

}