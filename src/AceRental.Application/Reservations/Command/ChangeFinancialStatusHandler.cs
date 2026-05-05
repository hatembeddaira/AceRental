using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AceRental.Application.Reservations.Dtos;
using AutoMapper;
using AceRental.Domain.Enum;
using System.Text.Json;
using AceRental.Domain.Extensions;
using AceRental.Application.Exceptions;

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
            if (reservation == null) 
                throw new NotFoundException(nameof(Reservation), request.ReservationId);

            // 1. Validation de la transition
            if (!reservation.FinancialStatus.CanTransitionTo(request.Status, reservation))
                throw new BusinessRuleException($"Transition impossible de {reservation?.FinancialStatus} vers {request.Status} " +
                $"dans le workflow {reservation!.Workflow} avec un statut logistique = {reservation!.LogisticStatus}");

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

            await AutoDeclancheAsync(request.Status, reservation, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        private async Task AutoDeclancheAsync(FinancialStatus status, Reservation reservation, CancellationToken cancellationToken)
        {
            switch (status)
            {
                case FinancialStatus.PartiallyPaid:
                    {
                        // Déclenchement automatique du PartiallyInvoiced si le statut devient PartiallyPaid
                        await _mediator.Send(new ChangeFinancialStatusCommand(){ ReservationId = reservation.Id, Status = FinancialStatus.PartiallyInvoiced }, cancellationToken);
                    }
                    ;
                    break;
                case FinancialStatus.PartiallyInvoiced:
                    {
                        // Génération de la facture partielle
                        // await _mediator.Send(new GeneratePartiallyInvoiceCommand(reservation.Id), cancellationToken);
                    }
                    ;
                    break;
                case FinancialStatus.RentalInvoiced:
                    {
                        // Génération de la facture de location
                        // await _mediator.Send(new GenerateRentalInvoiceCommand(reservation.Id), cancellationToken);

                        if (reservation.Workflow == Workflow.B2C && reservation.LogisticStatus == LogisticStatus.Checked)
                        {
                            await _mediator.Send(new ChangeLogisticStatusCommand(){ ReservationId = reservation.Id, Status = LogisticStatus.Finished }, cancellationToken);
                        }
                    }
                    ;
                    break;
                case FinancialStatus.Paid:
                    {
                        if (reservation.Workflow == Workflow.B2B && reservation.LogisticStatus == LogisticStatus.Checked)
                        {
                            await _mediator.Send(new ChangeLogisticStatusCommand(){ ReservationId = reservation.Id, Status = LogisticStatus.Finished }, cancellationToken);
                        }
                        else if (reservation.LogisticStatus == LogisticStatus.Damaged)
                        {
                            await _mediator.Send(new ChangeLogisticStatusCommand(){ ReservationId = reservation.Id, Status = LogisticStatus.Finished }, cancellationToken);
                        }
                        else
                        {
                            throw new BusinessRuleException($"Comportement non géré pour le statut financier {reservation.FinancialStatus}.");
                        }
                    }
                    ;
                    break;
                case FinancialStatus.Refunded:
                    {
                        // Déclenchement automatique du Refunded
                        // await _mediator.Send(new GenerateRefundCommand(reservation.Id), cancellationToken);
                    }
                    ;
                    break;
                default:
                    break;
            }
        }

    }
}