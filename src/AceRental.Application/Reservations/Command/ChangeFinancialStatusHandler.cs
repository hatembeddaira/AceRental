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

        public ChangeFinancialStatusHandler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<bool> Handle(ChangeFinancialStatusCommand request, CancellationToken cancellationToken)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);            
            if (reservation == null) throw new Exception($"Réservation ID {request.ReservationId} indisponible.");
            
            

            // 1. Validation de la transition
            if (!reservation.FinancialStatus.CanTransitionTo(request.Status, reservation))
                throw new Exception($"Transition impossible de {reservation?.FinancialStatus} vers {request.Status} dans le workflow {reservation!.Workflow} avec un statut logistique = {reservation!.LogisticStatus}");

            //  // 2. Appliquer les règles croisées avec le Workflow
            // if (request.Status == FinancialStatus.Paid && !reservation.CanMarkAsPaid())
            //     throw new InvalidOperationException("Paiement impossible dans l'état actuel (vérifiez le workflow).");

            // if (request.Status == FinancialStatus.Invoiced && !reservation.CanIssueInvoice())
            //     throw new InvalidOperationException("Facturation impossible (Le matériel doit être Checked en B2B).");
                
            // 2. Snapshot minimal pour l'historique
            var historyEntry = new ReservationHistoryDto {
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

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        
    }
}