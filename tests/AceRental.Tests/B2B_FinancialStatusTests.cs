using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AceRental.Tests
{
    public class B2B_FinancialStatusTests
    {

        // Tests de validation des transitions de statut logistique en workflow B2B
        // PartiallyPaid pas encore autorisé pour les transitions logistiques (ex: emporter) : 
        // on exige un paiement complet pour éviter les risques de non-paiement
        // Helper pour créer une réservation de base
        private Reservation CreateReservation(LogisticStatus log, FinancialStatus fin)
        {
            return new Reservation
            {
                ReservationNumber = 123,
                Workflow = Workflow.B2B,
                LogisticStatus = log,
                FinancialStatus = fin,
                Client = null! // Non nécessaire pour ces tests
            };
        }
        
    }
}