using AceRental.Domain.Entities;
using AceRental.Domain.Enum;
using AceRental.Domain.Extensions;
using Microsoft.VisualBasic;
using Xunit;

namespace AceRental.Tests
{
    public class B2C_LogisticStatusTests
    {

        // Tests de validation des transitions de statut logistique en workflow B2C
        // PartiallyPaid pas encore autorisé pour les transitions logistiques (ex: emporter) : 
        // on exige un paiement complet pour éviter les risques de non-paiement
        // Helper pour créer une réservation de base
        private Reservation CreateReservation(LogisticStatus log, FinancialStatus fin)
        {
            return new Reservation
            {
                ReservationNumber = 123,
                Workflow = Workflow.B2C,
                LogisticStatus = log,
                FinancialStatus = fin,
                Client = null! // Non nécessaire pour ces tests
            };
        }
        #region Block Transitions From List
        [Theory]
        [InlineData(LogisticStatus.Draft)]
        [InlineData(LogisticStatus.Quote)]
        [InlineData(LogisticStatus.Deleted)]
        [InlineData(LogisticStatus.Finished)]
        public void B2C_Should_Always_Block_Invalid_Transitions_From_List(LogisticStatus illegalTarget)
        {
            // On boucle sur TOUS les statuts logistiques possibles
            foreach (LogisticStatus log in Enum.GetValues<LogisticStatus>())
            {
                // On boucle sur TOUS les statuts financiers possibles
                foreach (FinancialStatus fin in Enum.GetValues<FinancialStatus>())
                {
                    // Arrange
                    var res = CreateReservation(illegalTarget, fin);

                    // Act
                    bool canTransition = res.LogisticStatus.CanTransitionTo(log, res);

                    // Assert
                    Assert.False(canTransition, 
                        $"Erreur : La transition {illegalTarget} -> {log} devrait être interdite pour le statut financier {fin}");
                }
            }
        }
        #endregion
        #region Transition From Basket 
        [Theory]
        [InlineData(FinancialStatus.Unpaid, true)]
        [InlineData(FinancialStatus.PartiallyPaid, false)]
        [InlineData(FinancialStatus.Paid, false)]
        [InlineData(FinancialStatus.PartiallyInvoiced, false)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, false)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_Basket_To_Confirmed(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.Basket, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.Confirmed, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(FinancialStatus.Unpaid, true)]
        [InlineData(FinancialStatus.PartiallyPaid, false)]
        [InlineData(FinancialStatus.Paid, false)]
        [InlineData(FinancialStatus.PartiallyInvoiced, false)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, false)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_Basket_To_Deleted(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.Basket, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.Deleted, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(LogisticStatus.Draft)]
        [InlineData(LogisticStatus.Quote)]
        [InlineData(LogisticStatus.Basket)]
        [InlineData(LogisticStatus.PickedUp)]
        [InlineData(LogisticStatus.Returned)]
        [InlineData(LogisticStatus.Checked)]
        [InlineData(LogisticStatus.Damaged)]
        [InlineData(LogisticStatus.Finished)]
        public void B2C_Should_Always_Block_Invalid_Transitions_From_Basket(LogisticStatus illegalTarget)
        {
            // On boucle sur TOUS les statuts financiers possibles
            foreach (FinancialStatus fin in Enum.GetValues<FinancialStatus>())
            {
                // Arrange
                var res = CreateReservation(LogisticStatus.Basket, fin);

                // Act
                bool canTransition = res.LogisticStatus.CanTransitionTo(illegalTarget, res);

                // Assert
                Assert.False(canTransition, 
                    $"Erreur : La transition Basket -> {illegalTarget} devrait être interdite pour le statut financier {fin}");
            }
        }
        #endregion
        #region Transition From Cancelled 
        [Theory]
        [InlineData(FinancialStatus.Unpaid, true)]
        [InlineData(FinancialStatus.PartiallyPaid, false)]
        [InlineData(FinancialStatus.Paid, false)]
        [InlineData(FinancialStatus.PartiallyInvoiced, false)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, false)]
        [InlineData(FinancialStatus.Refunded, true)]
        public void B2C_Should_Can_Transition_From_Cancelled_To_Basket(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.Cancelled, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.Basket, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(LogisticStatus.Draft)]
        [InlineData(LogisticStatus.Deleted)]
        [InlineData(LogisticStatus.Quote)]
        [InlineData(LogisticStatus.Confirmed)]
        [InlineData(LogisticStatus.PickedUp)]
        [InlineData(LogisticStatus.Returned)]
        [InlineData(LogisticStatus.Checked)]
        [InlineData(LogisticStatus.Damaged)]
        [InlineData(LogisticStatus.Finished)]
        public void B2C_Should_Always_Block_Invalid_Transitions_From_Cancelled(LogisticStatus illegalTarget)
        {
            // On boucle sur TOUS les statuts financiers possibles
            foreach (FinancialStatus fin in Enum.GetValues<FinancialStatus>())
            {
                // Arrange
                var res = CreateReservation(LogisticStatus.Cancelled, fin);

                // Act
                bool canTransition = res.LogisticStatus.CanTransitionTo(illegalTarget, res);

                // Assert
                Assert.False(canTransition, 
                    $"Erreur : La transition Cancelled -> {illegalTarget} devrait être interdite pour le statut financier {fin}");
            }
        }
        #endregion
        #region Transition From Confirmed 
        [Theory]
        [InlineData(FinancialStatus.Unpaid, true)]
        [InlineData(FinancialStatus.PartiallyPaid, true)]
        [InlineData(FinancialStatus.Paid, true)]
        [InlineData(FinancialStatus.PartiallyInvoiced, true)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, false)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_Confirmed_To_Cancelled(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.Confirmed, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.Cancelled, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(FinancialStatus.Unpaid, false)]
        [InlineData(FinancialStatus.PartiallyPaid, false)]
        [InlineData(FinancialStatus.Paid, true)]
        [InlineData(FinancialStatus.PartiallyInvoiced, false)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, false)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_Confirmed_To_PickedUp(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.Confirmed, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.PickedUp, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(LogisticStatus.Draft)]
        [InlineData(LogisticStatus.Deleted)]
        [InlineData(LogisticStatus.Quote)]
        [InlineData(LogisticStatus.Basket)]
        [InlineData(LogisticStatus.Confirmed)]
        [InlineData(LogisticStatus.Returned)]
        [InlineData(LogisticStatus.Checked)]
        [InlineData(LogisticStatus.Damaged)]
        [InlineData(LogisticStatus.Finished)]
        public void B2C_Should_Always_Block_Invalid_Transitions_From_Confirmed(LogisticStatus illegalTarget)
        {
            // On boucle sur TOUS les statuts financiers possibles
            foreach (FinancialStatus fin in Enum.GetValues<FinancialStatus>())
            {
                // Arrange
                var res = CreateReservation(LogisticStatus.Confirmed, fin);

                // Act
                bool canTransition = res.LogisticStatus.CanTransitionTo(illegalTarget, res);

                // Assert
                Assert.False(canTransition, 
                    $"Erreur : La transition Confirmed -> {illegalTarget} devrait être interdite pour le statut financier {fin}");
            }
        }
        #endregion
        #region Transition From PickedUp
        [Theory]
        [InlineData(FinancialStatus.Unpaid, false)]
        [InlineData(FinancialStatus.PartiallyPaid, false)]
        [InlineData(FinancialStatus.Paid, true)]
        [InlineData(FinancialStatus.PartiallyInvoiced, false)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, false)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_PickedUp_To_Returned(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.PickedUp, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.Returned, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(LogisticStatus.Draft)]
        [InlineData(LogisticStatus.Deleted)]
        [InlineData(LogisticStatus.Quote)]
        [InlineData(LogisticStatus.Basket)]
        [InlineData(LogisticStatus.Confirmed)]
        [InlineData(LogisticStatus.PickedUp)]
        [InlineData(LogisticStatus.Checked)]
        [InlineData(LogisticStatus.Damaged)]
        [InlineData(LogisticStatus.Finished)]
        public void B2C_Should_Always_Block_Invalid_Transitions_From_PickedUp(LogisticStatus illegalTarget)
        {
            // On boucle sur TOUS les statuts financiers possibles
            foreach (FinancialStatus fin in Enum.GetValues<FinancialStatus>())
            {
                // Arrange
                var res = CreateReservation(LogisticStatus.PickedUp, fin);

                // Act
                bool canTransition = res.LogisticStatus.CanTransitionTo(illegalTarget, res);

                // Assert
                Assert.False(canTransition, 
                    $"Erreur : La transition PickedUp -> {illegalTarget} devrait être interdite pour le statut financier {fin}");
            }
        }
        #endregion
        #region Transition From Returned
        [Theory]
        [InlineData(FinancialStatus.Unpaid, false)]
        [InlineData(FinancialStatus.PartiallyPaid, false)]
        [InlineData(FinancialStatus.Paid, true)]
        [InlineData(FinancialStatus.PartiallyInvoiced, false)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, false)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_Returned_To_Checked(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.Returned, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.Checked, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(LogisticStatus.Draft)]
        [InlineData(LogisticStatus.Deleted)]
        [InlineData(LogisticStatus.Quote)]
        [InlineData(LogisticStatus.Basket)]
        [InlineData(LogisticStatus.Confirmed)]
        [InlineData(LogisticStatus.PickedUp)]
        [InlineData(LogisticStatus.Returned)]
        [InlineData(LogisticStatus.Damaged)]
        [InlineData(LogisticStatus.Finished)]
        public void B2C_Should_Always_Block_Invalid_Transitions_From_Returned(LogisticStatus illegalTarget)
        {
            // On boucle sur TOUS les statuts financiers possibles
            foreach (FinancialStatus fin in Enum.GetValues<FinancialStatus>())
            {
                // Arrange
                var res = CreateReservation(LogisticStatus.Returned, fin);

                // Act
                bool canTransition = res.LogisticStatus.CanTransitionTo(illegalTarget, res);

                // Assert
                Assert.False(canTransition, 
                    $"Erreur : La transition Returned -> {illegalTarget} devrait être interdite pour le statut financier {fin}");
            }
        }
        #endregion
        #region Transition From Checked
        [Theory]
        [InlineData(FinancialStatus.Unpaid, false)]
        [InlineData(FinancialStatus.PartiallyPaid, false)]
        [InlineData(FinancialStatus.Paid, true)]
        [InlineData(FinancialStatus.PartiallyInvoiced, false)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, false)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_Checked_To_Damaged(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.Checked, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.Damaged, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(FinancialStatus.Unpaid, false)]
        [InlineData(FinancialStatus.PartiallyPaid, false)]
        [InlineData(FinancialStatus.Paid, false)]
        [InlineData(FinancialStatus.PartiallyInvoiced, false)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, true)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_Checked_To_Finished(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.Checked, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.Finished, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(LogisticStatus.Draft)]
        [InlineData(LogisticStatus.Deleted)]
        [InlineData(LogisticStatus.Quote)]
        [InlineData(LogisticStatus.Basket)]
        [InlineData(LogisticStatus.Confirmed)]
        [InlineData(LogisticStatus.PickedUp)]
        [InlineData(LogisticStatus.Returned)]
        [InlineData(LogisticStatus.Checked)]
        public void B2C_Should_Always_Block_Invalid_Transitions_From_Checked(LogisticStatus illegalTarget)
        {
            // On boucle sur TOUS les statuts financiers possibles
            foreach (FinancialStatus fin in Enum.GetValues<FinancialStatus>())
            {
                // Arrange
                var res = CreateReservation(LogisticStatus.Checked, fin);

                // Act
                bool canTransition = res.LogisticStatus.CanTransitionTo(illegalTarget, res);

                // Assert
                Assert.False(canTransition, 
                    $"Erreur : La transition Checked -> {illegalTarget} devrait être interdite pour le statut financier {fin}");
            }
        }
        #endregion
        #region Transition From Damaged
        [Theory]
        [InlineData(FinancialStatus.Unpaid, false)]
        [InlineData(FinancialStatus.PartiallyPaid, false)]
        [InlineData(FinancialStatus.Paid, true)]
        [InlineData(FinancialStatus.PartiallyInvoiced, false)]
        [InlineData(FinancialStatus.RepairRentalInvoiced, false)]
        [InlineData(FinancialStatus.RentalInvoiced, false)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_Damaged_To_Finished(FinancialStatus financial, bool excepted)
        {
            // Arrange
            var res = CreateReservation(LogisticStatus.Damaged, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(LogisticStatus.Finished, res);
            // Assert
            Assert.Equal(excepted, result);
        }

        [Theory]
        [InlineData(LogisticStatus.Draft)]
        [InlineData(LogisticStatus.Deleted)]
        [InlineData(LogisticStatus.Quote)]
        [InlineData(LogisticStatus.Basket)]
        [InlineData(LogisticStatus.Confirmed)]
        [InlineData(LogisticStatus.PickedUp)]
        [InlineData(LogisticStatus.Returned)]
        [InlineData(LogisticStatus.Checked)]
        [InlineData(LogisticStatus.Damaged)]
        public void B2C_Should_Always_Block_Invalid_Transitions_From_Damaged(LogisticStatus illegalTarget)
        {
            // On boucle sur TOUS les statuts financiers possibles
            foreach (FinancialStatus fin in Enum.GetValues<FinancialStatus>())
            {
                // Arrange
                var res = CreateReservation(LogisticStatus.Damaged, fin);

                // Act
                bool canTransition = res.LogisticStatus.CanTransitionTo(illegalTarget, res);

                // Assert
                Assert.False(canTransition, 
                    $"Erreur : La transition Damaged -> {illegalTarget} devrait être interdite pour le statut financier {fin}");
            }
        }
        #endregion
    }
}