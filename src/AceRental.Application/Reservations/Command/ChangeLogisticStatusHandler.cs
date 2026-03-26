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
    public class ChangeLogisticStatusHandler : IRequestHandler<ChangeLogisticStatusCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        // private readonly IUnitOfWork _unitOfWork;

        public ChangeLogisticStatusHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(ChangeLogisticStatusCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);            
            if (reservation == null) throw new Exception($"Réservation ID {request.ReservationId} indisponible.");
            
            

            // 1. Validation de la transition
            if (!reservation.LogisticStatus.CanTransitionTo(request.Status, reservation))
                throw new Exception($"Transition impossible de {reservation?.LogisticStatus} vers {request.Status} dans le workflow {reservation!.Workflow} avec un statut financière = {reservation!.FinancialStatus}");
            // if (request.Status == LogisticStatus.Finished && !reservation.CanMarkAsFinished())
            //     throw new Exception($"Ferméture impossible dans l'état actuel (vérifiez le workflow).");

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

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        
    }
}