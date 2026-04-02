using MediatR;
using Microsoft.EntityFrameworkCore;
using AceRental.Domain.Entities;
using AceRental.Infrastructure.Persistence;
using AceRental.Application.Reservations.Dtos;
using AutoMapper;
using AceRental.Domain.Enum;
using System.Text.Json;
using AceRental.Domain.Extensions;
using AceRental.Application.Payments.Queries;

namespace AceRental.Application.Reservations.Command
{
    public class ChangeLogisticStatusHandler : IRequestHandler<ChangeLogisticStatusCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        // private readonly IUnitOfWork _unitOfWork;

        public ChangeLogisticStatusHandler(ApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<bool> Handle(ChangeLogisticStatusCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);            
            if (reservation == null) throw new Exception($"Réservation ID {request.ReservationId} indisponible.");
            
            

            // 1. Validation de la transition
            if (!reservation.LogisticStatus.CanTransitionTo(request.Status, reservation))
                throw new Exception($"Transition impossible de {reservation?.LogisticStatus} vers {request.Status} dans le workflow {reservation!.Workflow} avec un statut financière = {reservation!.FinancialStatus}");

            // 2. Snapshot minimal pour l'historique
            var historyEntry = new ReservationHistoryDto {
                ReservationId = reservation.Id,
                HistoryType = HistoryType.Logistic,
                VersionNumber = reservation.CurrentVersion,
                ChangeReason = $"Changement de statut",
                DataSnapshotJson = JsonSerializer.Serialize(new { Old = reservation.LogisticStatus.ToString(), New = request.Status.ToString() })
            };
            if(request.Status == LogisticStatus.Deleted)
            {
                // Si le statut devient "Deleted", on ajoute une raison de suppression
                historyEntry.ChangeReason = $"Suppression de la réservation";
                _context.Reservations.Remove(reservation);
            }

            _context.ReservationHistorys.Add(_mapper.Map<ReservationHistory>(historyEntry));
            reservation.LogisticStatus = request.Status;
            reservation.CurrentVersion++;

            await AutoDeclancheAsync(request.Status, reservation, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        private async Task  AutoDeclancheAsync(LogisticStatus status, Reservation reservation, CancellationToken cancellationToken)
        {
            switch (status)
            {
                case LogisticStatus.Cancelled:
                    {
                        // Déclenchement automatique du Refunded s'il avait des paiements
                        var resultPayments = await _mediator.Send(new GetAllPaymentsQuery(reservation.Id), cancellationToken);
                        if(resultPayments.Any() && resultPayments.Sum(p => p.Amount) > 0)
                        {
                            await _mediator.Send(new ChangeFinancialStatusCommand(reservation.Id, FinancialStatus.Refunded), cancellationToken);
                        }
                    }
                    ;
                    break;
                case LogisticStatus.PickedUp:
                    {   
                        // envoi de notification ou email au client pour lui tienr informé que sa commande est expédiée
                    }
                    ;
                    break;
                case LogisticStatus.Returned:
                    {   
                        // envoi de notification ou email au SAV pour qu'il puisse faire le suivi de l'état du matériel retourné 
                        // et déclencher les actions nécessaires (Checked, Damaged.)
                    }
                    ;
                    break;
                case LogisticStatus.Checked:
                    {   
                        // Génération de la facture de location
                        await _mediator.Send(new ChangeFinancialStatusCommand(reservation.Id, FinancialStatus.RentalInvoiced), cancellationToken);
                    }
                    ;
                    break;
                case LogisticStatus.Damaged:
                    {
                        // envoi de notification ou email au commercial pour qu'il puisse faire la facture de réparation pour le matériel endommagé
                    }
                    ;
                    break;
                default:
                    break;
            }
        }       
    }
}