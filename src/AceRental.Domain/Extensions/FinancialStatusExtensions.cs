using AceRental.Domain.Enum;
using AceRental.Domain.Entities;

namespace AceRental.Domain.Extensions
{
    public static class FinancialStatusExtensions
    {
        private static readonly List<LogisticStatus> AllowedLogisticStatusesForPartiallyPaid = new List<LogisticStatus>
        {
            LogisticStatus.Confirmed,
            LogisticStatus.PickedUp,
            LogisticStatus.Returned
        };
        public static bool CanTransitionTo(this FinancialStatus current, FinancialStatus next, Reservation reservation)
        {
            return current switch
            {
                FinancialStatus.Unpaid => CanTransitionFromUnpaid(next, reservation),
                FinancialStatus.PartiallyPaid => CanTransitionFromPartiallyPaid(next, reservation),
                FinancialStatus.Paid => CanTransitionFromPaid(next, reservation),
                FinancialStatus.PartiallyInvoiced => CanTransitionFromPartiallyInvoiced(next, reservation),
                FinancialStatus.RepairRentalInvoiced => CanTransitionFromRepairRentalInvoiced(next, reservation),
                FinancialStatus.RentalInvoiced => CanTransitionFromRentalInvoiced(next, reservation),
                FinancialStatus.Refunded => false,
                _ => false
            };
        }

        #region FinancialStatus Transitions
        private static bool CanTransitionFromUnpaid(FinancialStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromUnpaidB2C(next, reservation),
                Workflow.B2B => CanTransitionFromUnpaidB2B(next, reservation),
                _ => false
            };
        }
        private static bool CanTransitionFromPartiallyPaid(FinancialStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromPartiallyPaidB2C(next, reservation),
                Workflow.B2B => CanTransitionFromPartiallyPaidB2B(next, reservation),
                _ => false
            };
        }
        private static bool CanTransitionFromPaid(FinancialStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromPaidB2C(next, reservation),
                Workflow.B2B => false,
                _ => false
            };
        }
        private static bool CanTransitionFromPartiallyInvoiced(FinancialStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromPartiallyInvoicedB2C(next, reservation),
                Workflow.B2B => CanTransitionFromPartiallyInvoicedB2B(next, reservation),
                _ => false
            };
        }
        private static bool CanTransitionFromRepairRentalInvoiced(FinancialStatus next, Reservation reservation)
        {
            return next == FinancialStatus.Paid && reservation.LogisticStatus == LogisticStatus.Damaged;
        }        
        private static bool CanTransitionFromRentalInvoiced(FinancialStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => false,
                Workflow.B2B => next == FinancialStatus.Paid && reservation.LogisticStatus == LogisticStatus.Checked,
                _ => false
            };
        }
        #endregion

        #region B2C
        private static bool CanTransitionFromUnpaidB2C(FinancialStatus next, Reservation reservation)
        {
            return next switch
            {
                FinancialStatus.PartiallyPaid => reservation.LogisticStatus == LogisticStatus.Confirmed,
                FinancialStatus.Paid => reservation.LogisticStatus == LogisticStatus.Confirmed,
                _ => false
            };
        }
        private static bool CanTransitionFromPartiallyPaidB2C(FinancialStatus next, Reservation reservation)
        {
            return next == FinancialStatus.PartiallyInvoiced && reservation.LogisticStatus == LogisticStatus.Confirmed;
        }
        private static bool CanTransitionFromPaidB2C(FinancialStatus next, Reservation reservation)
        {
            return next switch
            {
                FinancialStatus.RentalInvoiced => reservation.LogisticStatus == LogisticStatus.Checked,
                FinancialStatus.RepairRentalInvoiced => reservation.LogisticStatus == LogisticStatus.Damaged,
                _ => false
            };
        }
        private static bool CanTransitionFromPartiallyInvoicedB2C(FinancialStatus next, Reservation reservation)
        {
            return next == FinancialStatus.Paid && reservation.LogisticStatus == LogisticStatus.Confirmed;
        }
        
        #endregion

        #region B2B
        private static bool CanTransitionFromUnpaidB2B(FinancialStatus next, Reservation reservation)
        {
            return next switch
            {
                FinancialStatus.PartiallyPaid => reservation.LogisticStatus == LogisticStatus.Confirmed,
                FinancialStatus.RentalInvoiced => AllowedLogisticStatusesForPartiallyPaid.Contains(reservation.LogisticStatus),
                _ => false
            };
        }
        private static bool CanTransitionFromPartiallyPaidB2B(FinancialStatus next, Reservation reservation)
        {
            return next switch
            {
                FinancialStatus.PartiallyInvoiced => 
                    reservation.LogisticStatus == LogisticStatus.Confirmed || 
                    reservation.LogisticStatus == LogisticStatus.PickedUp ||
                    reservation.LogisticStatus == LogisticStatus.Returned,
                _ => false
            };
        }
        private static bool CanTransitionFromPartiallyInvoicedB2B(FinancialStatus next, Reservation reservation)
        {
            return next switch
            {
                FinancialStatus.PartiallyPaid => 
                    reservation.LogisticStatus == LogisticStatus.PickedUp ||
                    reservation.LogisticStatus == LogisticStatus.Returned,
                FinancialStatus.RentalInvoiced =>  reservation.LogisticStatus == LogisticStatus.Checked,
                FinancialStatus.RepairRentalInvoiced =>  reservation.LogisticStatus == LogisticStatus.Damaged,
                _ => false
            };
        }
        #endregion
    }
}