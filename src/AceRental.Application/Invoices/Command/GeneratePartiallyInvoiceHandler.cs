using MediatR;
using AceRental.Infrastructure.Persistence;
using AutoMapper;
using AceRental.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using AceRental.Application.Invoices.Dtos;
using AceRental.Domain.Entities;
using AceRental.Application.Exceptions;

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

            var partiallyInvoice = new InvoiceDto
            {
                Id = Guid.NewGuid(),
                ReservationId = reservation.Id,
                Type = InvoiceType.PartiallyInvoice,
                AmountHT = reservation.TotalHT
            };

            reservation.FinancialStatus = FinancialStatus.PartiallyInvoiced;

            _context.Invoices.Add(_mapper.Map<Invoice>(partiallyInvoice));
            await _context.SaveChangesAsync(cancellationToken);
            // Send Mail or Notification 
            return partiallyInvoice.Id;
        }
    }
}