using AceRental.Domain.Entities;
using AceRental.Domain.Enum;
using AceRental.Domain.Extensions;
using Xunit;

namespace AceRental.Tests.Domain
{
    public class B2C_ReservationStatusTests
    {
        // Tests de validation des transitions de statut logistique en workflow B2C
        // PartiallyPaid pas encore autorisé pour les transitions logistiques (ex: emporter) : 
        // on exige un paiement complet pour éviter les risques de non-paiement
        // Helper pour créer une réservation de base
        private Reservation CreateReservation(Workflow workflow, LogisticStatus log, FinancialStatus fin)
        {
            return new Reservation
            {
                ReservationNumber = 123,
                Workflow = workflow,
                LogisticStatus = log,
                FinancialStatus = fin,
                Client = null! // Non nécessaire pour ces tests
            };
        }

        [Theory]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Confirmed, true)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Confirmed, false)] // Acompte versé pas encore autorisé
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Cancelled, true)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, true)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2C_Should_Can_Transition_From_Basket_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.Basket, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Confirmed, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Cancelled, true)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Cancelled, false)] // Annulation possible avec acompte (remboursement à suivre) pas encore autorisé
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, false)]
        // [InlineData(FinancialStatus.Invoiced, LogisticStatus.PickedUp, true)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.PickedUp, false)] // Bloqué : solde requis pour emporter
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2C_Should_Can_Transition_From_Confirmed_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.Confirmed, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }


        [Theory]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Confirmed, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Cancelled, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, true)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Deleted, false)] // Sécurité : on ne supprime pas si argent encaissé
        [InlineData(FinancialStatus.Paid, LogisticStatus.Deleted, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, true)]
        [InlineData(FinancialStatus.Paid, LogisticStatus.Basket, false)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Basket, false)] // Sécurité : retour au panier interdit si acompte
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2C_Should_Can_Transition_From_Cancelled_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.Cancelled, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }


        [Theory]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Confirmed, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Cancelled, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2C_Should_Can_Transition_From_Deleted_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.Deleted, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }


        [Theory]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Confirmed, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Cancelled, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Paid, LogisticStatus.Returned, true)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2C_Should_Can_Transition_From_PickedUp_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.PickedUp, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }


        [Theory]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Confirmed, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Cancelled, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Paid, LogisticStatus.Checked, true)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2C_Should_Can_Transition_From_Returned_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.Returned, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }


        [Theory]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Confirmed, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Cancelled, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        // [InlineData(FinancialStatus.Invoiced, LogisticStatus.Finished, true)]     // Clôture autorisée si facturé/soldé
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Finished, false)] // Bloqué : il reste un solde à payer
        public void B2C_Should_Can_Transition_From_Checked_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.Checked, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }



        // [Theory]
        // [InlineData(FinancialStatus.Unpaid, false)]
        // [InlineData(FinancialStatus.PartiallyPaid, false)] // Unpaid -> PartiallyPaid (Acompte ou tranche 1,2/3) pas encore autorisé
        // [InlineData(FinancialStatus.Paid, true)]          // Unpaid -> Paid (100%)
        // // [InlineData(FinancialStatus.Invoiced, false)]
        // [InlineData(FinancialStatus.Refunded, false)]
        // public void B2C_Should_Can_Transition_From_Confirmed_To_FinancialStatus(FinancialStatus next, bool excepted)
        // {
        //     // Arrange
        //     var res = CreateReservation(Workflow.B2C, LogisticStatus.Confirmed, FinancialStatus.Unpaid);
        //     // Act:
        //     bool result = res.LogisticStatus.CanTransitionTo(next, res);
        //     // Assert
        //     Assert.Equal(excepted, result);
        // }
        
        [Theory] // Transitions financières PartiallyPaid pas encore autorisées
        [InlineData(FinancialStatus.Paid, false)]         // PartiallyPaid -> Paid (Dernière tranche reçue)
        // [InlineData(FinancialStatus.Invoiced, false)]     // PartiallyPaid -> Invoiced (Cas où on facture avant le solde)
        [InlineData(FinancialStatus.Unpaid, false)]       // Retour arrière interdit
        public void B2C_Should_Can_Transition_From_PartiallyPaid_To_FinancialStatus(FinancialStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.Confirmed, FinancialStatus.PartiallyPaid);
            // Act
            bool result = res.FinancialStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }
    }
}