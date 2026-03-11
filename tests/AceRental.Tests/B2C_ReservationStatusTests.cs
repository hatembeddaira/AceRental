using AceRental.Domain.Entities;
using AceRental.Domain.Enum;
using AceRental.Domain.Extensions;
using Xunit;

namespace AceRental.Tests.Domain
{
    public class B2C_ReservationStatusTests
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
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Draft, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Deleted, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, false)]
        [InlineData(FinancialStatus.Invoiced, LogisticStatus.PickedUp, true)]
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
        [InlineData(FinancialStatus.Paid, LogisticStatus.Deleted, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Quote, false)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Basket, true)]
        [InlineData(FinancialStatus.Paid, LogisticStatus.Basket, false)]
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
        [InlineData(FinancialStatus.Invoiced, LogisticStatus.Finished, true)]
        [InlineData(FinancialStatus.Unpaid, LogisticStatus.Finished, false)]
        public void B2C_Should_Can_Transition_From_Checked_To_LogisticStatus(FinancialStatus financial, LogisticStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.Checked, financial);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }



        [Theory]
        [InlineData(FinancialStatus.Unpaid, false)]
        [InlineData(FinancialStatus.Paid, true)]
        [InlineData(FinancialStatus.Invoiced, false)]
        [InlineData(FinancialStatus.Refunded, false)]
        public void B2C_Should_Can_Transition_From_Confirmed_To_FinancialStatus(FinancialStatus next, bool excepted)
        {
            // Arrange
            var res = CreateReservation(Workflow.B2C, LogisticStatus.Confirmed, FinancialStatus.Unpaid);
            // Act:
            bool result = res.LogisticStatus.CanTransitionTo(next, res);
            // Assert
            Assert.Equal(excepted, result);
        }

    }
}