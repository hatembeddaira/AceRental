using AceRental.Domain.Entities;
using AceRental.Domain.Enum;
using AceRental.Domain.Extensions;
using Xunit;

namespace AceRental.Tests.Domain
{
    public class B2B_ReservationStatusTests
    {
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
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Cancelled, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, true)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Deleted, false)] // Sécurité : On ne supprime pas si argent reçu
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, true)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2B_Should_Can_Transition_From_Draft_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Draft, financial);
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
        // [InlineData(FinancialStatus.Invoiced, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.PickedUp, true)] // Bloqué : solde requis pour emporter
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2B_Should_Can_Transition_From_Confirmed_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Confirmed, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }


        [Theory]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Confirmed, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Cancelled, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, true)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Draft, true)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, true)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Deleted, false)] // Sécurité : on ne supprime pas si argent encaissé
        [InlineData(FinancialStatus.Paid, LogisticStatus.Deleted, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.PickedUp, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2B_Should_Can_Transition_From_Cancelled_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Cancelled, financial);
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
        public void B2B_Should_Can_Transition_From_Deleted_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Deleted, financial);
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
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Returned, true)]
        [InlineData(FinancialStatus.Paid, LogisticStatus.Returned, true)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Returned, true)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2B_Should_Can_Transition_From_PickedUp_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2B, LogisticStatus.PickedUp, financial);
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
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Checked, true)]
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Checked, true)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2B_Should_Can_Transition_From_Returned_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Returned, financial);
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
        // [InlineData(FinancialStatus.Invoiced, LogisticStatus.Finished, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        [InlineData(FinancialStatus.Paid, LogisticStatus.Finished, true)] // Clôture autorisée si facturé/soldé
        [InlineData(FinancialStatus.PartiallyPaid, LogisticStatus.Finished, false)] // Bloqué : il reste un solde à payer
        public void B2B_Should_Can_Transition_From_Checked_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Checked, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }

 

        // [Theory]
        // [InlineData(FinancialStatus.Unpaid, false)]
        // // [InlineData(FinancialStatus.PartiallyPaid, true)] // Unpaid -> PartiallyPaid (Acompte ou tranche 1,2/3) pas encore autorisé
        // // [InlineData(FinancialStatus.Paid, false)]          // Unpaid -> Paid (100%)
        // // [InlineData(FinancialStatus.Invoiced, true)]
        // // [InlineData(FinancialStatus.Refunded, true)]
        // public void B2B_Should_Can_Transition_From_Confirmed_To_FinancialStatus(FinancialStatus next, bool excepted)
        // {
        //     // Arrange
        //     var res = CreateReservation(Workflow.B2B, LogisticStatus.Confirmed, FinancialStatus.Unpaid);
        //     // Act:
        //     bool result = res.LogisticStatus.CanTransitionTo(next, res);
        //     // Assert
        //     Assert.Equal(excepted, result);
        // }

        [Theory]
        [InlineData(LogisticStatus.Checked, FinancialStatus.Paid, true)]         // PartiallyPaid -> Paid (Dernière tranche reçue)
        // [InlineData(LogisticStatus.Confirmed, FinancialStatus.Invoiced, true)]      // PartiallyPaid -> Invoiced (Cas où on facture avant le solde)
        [InlineData(LogisticStatus.Confirmed, FinancialStatus.PartiallyPaid, true)]      // PartiallyPaid -> Invoiced (Cas où on facture avant le solde)
        [InlineData(LogisticStatus.Checked, FinancialStatus.Unpaid, false)]       // Retour arrière interdit
        public void B2B_Should_Can_Transition_From_PartiallyPaid_To_FinancialStatus(LogisticStatus logistic, FinancialStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2B, logistic, FinancialStatus.PartiallyPaid);
            // Act
            bool result = res.FinancialStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        
    }
}