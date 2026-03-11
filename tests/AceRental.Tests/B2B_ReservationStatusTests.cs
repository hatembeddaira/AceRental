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


        [Fact]
        public void B2B_Should_Block_Invoice_If_Not_Checked()
        {
            // Arrange : Matériel revenu mais PAS ENCORE vérifié
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Returned, FinancialStatus.Unpaid);

            // Act
            bool canInvoice = res.FinancialStatus.CanTransitionTo(FinancialStatus.Invoiced, res);

            // Assert
            Assert.False(canInvoice, "B2B ne devrait pas pouvoir facturer tant que le statut n'est pas Checked.");
        }

        [Fact]
        public void B2B_Should_Allow_Invoice_Once_Checked()
        {
            // Arrange : Matériel vérifié par le technicien
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Checked, FinancialStatus.Unpaid);

            // Act
            bool canInvoice = res.FinancialStatus.CanTransitionTo(FinancialStatus.Invoiced, res);

            // Assert
            Assert.True(canInvoice, "B2B doit pouvoir facturer une fois le matériel vérifié.");
        }

        [Fact]
        public void B2B_Should_Block_Payment_If_Not_Invoiced()
        {
            // Arrange
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Checked, FinancialStatus.Unpaid);

            // Act
            bool canPay = res.FinancialStatus.CanTransitionTo(FinancialStatus.Paid, res);

            // Assert
            Assert.False(canPay, "B2B ne peut pas payer sans facture (Invoiced) préalable.");
        }

        [Theory]
        [InlineData(Workflow.B2B, LogisticStatus.Draft)]
        [InlineData(Workflow.B2B, LogisticStatus.Quote)]
        public void B2B_Should_Allow_Back_To_Draft_Only_From_Cancelled(Workflow workflow, LogisticStatus current)
        {
            // Arrange
            var res = CreateReservation(workflow, current, FinancialStatus.Unpaid);

            // Act: Essai direct vers Draft
            bool directToDraft = res.LogisticStatus.CanTransitionTo(LogisticStatus.Draft, res);
            
            // Assert
            Assert.False(directToDraft, "Le passage direct vers Draft doit être bloqué pour la traçabilité.");

            // Act: Passage par Cancelled
            res.LogisticStatus = LogisticStatus.Cancelled;
            bool fromCancelledToDraft = res.LogisticStatus.CanTransitionTo(LogisticStatus.Draft, res);
            
            Assert.True(fromCancelledToDraft);
        }

        [Fact]
        public void B2B_Should_Block_Finished_If_Not_Paid()
        {
            // Arrange : Matériel vérifié et facturé, mais virement non reçu
            var res = CreateReservation(Workflow.B2B, LogisticStatus.Checked, FinancialStatus.Invoiced);

            // Act
            bool canFinish = res.LogisticStatus.CanTransitionTo(LogisticStatus.Finished, res);

            // Assert
            Assert.False(canFinish, "B2B : On ne ferme pas le dossier tant que l'argent n'est pas encaissé.");
            
            // Simule paiement reçu
            res.FinancialStatus = FinancialStatus.Paid;
            Assert.True(res.LogisticStatus.CanTransitionTo(LogisticStatus.Finished, res));
        }
    }
}