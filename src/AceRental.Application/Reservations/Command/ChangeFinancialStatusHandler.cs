using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AceRental.Application.Reservations.Dtos;
using AutoMapper;
using AceRental.Domain.Enum;
using System.Text.Json;
using AceRental.Domain.Extensions;

namespace AceRental.Application.Reservations.Command
{
    public class ChangeFinancialStatusHandler : IRequestHandler<ChangeFinancialStatusCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public ChangeFinancialStatusHandler(ApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<bool> Handle(ChangeFinancialStatusCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);
            if (reservation == null) throw new Exception($"Réservation ID {request.ReservationId} indisponible.");



            // 1. Validation de la transition
            if (!reservation.FinancialStatus.CanTransitionTo(request.Status, reservation))
                throw new Exception($"Transition impossible de {reservation?.FinancialStatus} vers {request.Status} dans le workflow {reservation!.Workflow} avec un statut logistique = {reservation!.LogisticStatus}");

            // 2. Snapshot minimal pour l'historique
            var historyEntry = new ReservationHistoryDto
            {
                ReservationId = reservation.Id,
                HistoryType = HistoryType.Financial,
                VersionNumber = reservation.CurrentVersion,
                ChangeReason = "Changement de statut",
                DataSnapshotJson = JsonSerializer.Serialize(new { Old = reservation.FinancialStatus.ToString(), New = request.Status.ToString() })
            };
            _context.ReservationHistorys.Add(_mapper.Map<ReservationHistory>(historyEntry));
            // 3. Application
            reservation.FinancialStatus = request.Status;
            reservation.CurrentVersion++;

            switch (reservation.Workflow)
            {
                case Workflow.B2C:
                    await AutoDeclancheB2CAsync(request.Status, reservation, cancellationToken);
                    break;
                case Workflow.B2B:
                    AutoDeclancheB2B(request.Status, reservation, cancellationToken);
                    break;
                default:
                    break;
            }

            // if(reservation.Workflow == Workflow.B2C && reservation.FinancialStatus == FinancialStatus.Paid)
            // {
            //     // Si c'est une réservation B2C et que le statut financier devient "Paid", on passe le statut financier à "Invoiced"
            //     var result = await _mediator.Send(new ChangeFinancialStatusCommand(request.ReservationId, FinancialStatus.Invoiced), cancellationToken);
            // }

            // if(reservation.Workflow == Workflow.B2C && reservation.FinancialStatus == FinancialStatus.Invoiced)
            // {
            //     // Si c'est une réservation B2C et que le statut financier devient "Invoiced", on génére la facture
            //     // var result = await Mediator.Send(new GenerateInvoiceHandler(request.ReservationId), cancellationToken);
            // }
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        private async Task  AutoDeclancheB2CAsync(FinancialStatus status, Reservation reservation, CancellationToken cancellationToken)
        {
            switch (status)
            {
                // case FinancialStatus.Cancelled:
                //     {
                //         // Déclenchement automatique du Refunded si le statut devient Cancelled
                //         var result = await _mediator.Send(new ChangeFinancialStatusCommand(reservation.Id, FinancialStatus.Refunded), cancellationToken);
                //     }
                //     ;
                //     break;
                case FinancialStatus.PartiallyPaid:
                    {
                        // Déclenchement automatique du PartiallyInvoiced si le statut devient PartiallyPaid
                        var result = await _mediator.Send(new ChangeFinancialStatusCommand(reservation.Id, FinancialStatus.PartiallyInvoiced), cancellationToken);
                    }
                    ;
                    break;
                case FinancialStatus.PartiallyInvoiced:
                    {   
                        // Génération de la facture partielle
                    }
                    ;
                    break;
                case FinancialStatus.Refunded:
                    {   
                        // Déclenchement automatique du Refunded
                    }
                    ;
                    break;
                default:
                    break;
            }
        }
        private void AutoDeclancheB2B(FinancialStatus status, Reservation reservation, CancellationToken cancellationToken)
        {

        }
    }
}