using AceRental.Domain.Enum;
using AceRental.Domain.Entities;

namespace AceRental.Domain.Extensions
{
    public static class LogisticStatusExtensions
    {

        private static readonly List<FinancialStatus> AllowedFinancialStatusFromConfirmedB2C = new List<FinancialStatus>
        {
            FinancialStatus.Unpaid,
            FinancialStatus.PartiallyPaid,
            FinancialStatus.PartiallyInvoiced,
            FinancialStatus.Paid
        };
        private static readonly List<FinancialStatus> AllowedFinancialStatusFromConfirmedB2B = new List<FinancialStatus>
        {
            FinancialStatus.Unpaid,
            FinancialStatus.PartiallyPaid,
            FinancialStatus.PartiallyInvoiced
        };
        public static bool CanTransitionTo(this LogisticStatus current, LogisticStatus next, Reservation reservation)
        {
            return current switch
            {
                // Entrée par le Commercial
                LogisticStatus.Draft => CanTransitionFromDraft(next, reservation),
                LogisticStatus.Quote => CanTransitionFromQuote(next, reservation),
                // Entrée par le Web
                LogisticStatus.Basket => CanTransitionFromBasket(next, reservation),
                // Cycle de vie actif
                LogisticStatus.Confirmed => CanTransitionFromConfirmed(next, reservation),
                LogisticStatus.PickedUp => CanTransitionFromPickedUp(next, reservation),
                // Cycle de clôture (Contrôle Qualité)
                LogisticStatus.Returned => CanTransitionFromReturned(next, reservation),
                LogisticStatus.Checked => CanTransitionFromChecked(next, reservation),
                LogisticStatus.Damaged => CanTransitionFromDamaged(next, reservation),
                // États finaux ou de sortie
                LogisticStatus.Cancelled => CanTransitionFromCancelled(next, reservation),
                LogisticStatus.Finished => false,
                LogisticStatus.Deleted => false,
                _ => false
            };
        }


        #region FinancialStatus Transitions
        private static bool CanTransitionFromDraft(LogisticStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => false,
                Workflow.B2B => CanTransitionFromDraftB2B(next, reservation),
                _ => false
            };
        }
        private static bool CanTransitionFromQuote(LogisticStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => false,
                Workflow.B2B => CanTransitionFromQuoteB2B(next, reservation),
                _ => false
            };
        }
        private static bool CanTransitionFromBasket(LogisticStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromBasketB2C(next, reservation),
                Workflow.B2B => false,
                _ => false
            };
        }
        private static bool CanTransitionFromConfirmed(LogisticStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromConfirmedB2C(next, reservation),
                Workflow.B2B => CanTransitionFromConfirmedB2B(next, reservation),
                _ => false
            };
        }
        private static bool CanTransitionFromPickedUp(LogisticStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromPickedUpB2C(next, reservation),
                Workflow.B2B => CanTransitionFromPickedUpB2B(next, reservation),
                _ => false
            };
        }
        private static bool CanTransitionFromReturned(LogisticStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromReturnedB2C(next, reservation),
                Workflow.B2B => CanTransitionFromReturnedB2B(next, reservation),
                _ => false
            };
        }
        private static bool CanTransitionFromChecked(LogisticStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromCheckedB2C(next, reservation),
                Workflow.B2B => CanTransitionFromCheckedB2B(next, reservation),
                _ => false
            };
        }
        private static bool CanTransitionFromDamaged(LogisticStatus next, Reservation reservation)
        {
            return next == LogisticStatus.Finished && reservation.FinancialStatus == FinancialStatus.Paid;
        }
        private static bool CanTransitionFromCancelled(LogisticStatus next, Reservation reservation)
        {
            return reservation.Workflow switch
            {
                Workflow.B2C => CanTransitionFromCancelledB2C(next, reservation),
                Workflow.B2B => CanTransitionFromCancelledB2B(next, reservation),
                _ => false
            };
        }
        #endregion
        #region B2C
        private static bool CanTransitionFromBasketB2C(LogisticStatus next, Reservation reservation)
        {
            return next switch
            {
                LogisticStatus.Confirmed => reservation.FinancialStatus == FinancialStatus.Unpaid,
                LogisticStatus.Deleted => reservation.FinancialStatus == FinancialStatus.Unpaid,
                _ => false
            };
        }
        private static bool CanTransitionFromConfirmedB2C(LogisticStatus next, Reservation reservation)
        {
            return next switch
            {
                LogisticStatus.Cancelled => AllowedFinancialStatusFromConfirmedB2C.Contains(reservation.FinancialStatus),
                LogisticStatus.PickedUp => reservation.FinancialStatus == FinancialStatus.Paid,
                _ => false
            };
        }
        private static bool CanTransitionFromPickedUpB2C(LogisticStatus next, Reservation reservation)
        {
            return next == LogisticStatus.Returned && reservation.FinancialStatus == FinancialStatus.Paid;
        }
        private static bool CanTransitionFromReturnedB2C(LogisticStatus next, Reservation reservation)
        {
            return next == LogisticStatus.Checked && reservation.FinancialStatus == FinancialStatus.Paid;
        }
        private static bool CanTransitionFromCheckedB2C(LogisticStatus next, Reservation reservation)
        {
            return next switch
            {
                LogisticStatus.Damaged => reservation.FinancialStatus == FinancialStatus.Paid,
                LogisticStatus.Finished => reservation.FinancialStatus == FinancialStatus.RentalInvoiced,
                _ => false
            };
        }
        private static bool CanTransitionFromDamagedB2C(LogisticStatus next, Reservation reservation)
        {
            return next == LogisticStatus.Finished && reservation.FinancialStatus == FinancialStatus.Paid;
        }
        private static bool CanTransitionFromCancelledB2C(LogisticStatus next, Reservation reservation)
        {
            return next == LogisticStatus.Basket && 
                (reservation.FinancialStatus == FinancialStatus.Unpaid || reservation.FinancialStatus == FinancialStatus.Refunded);
        }        
        #endregion
        #region B2B
        private static bool CanTransitionFromDraftB2B(LogisticStatus next, Reservation reservation)
        {
            return next switch
            {
                LogisticStatus.Deleted => reservation.FinancialStatus == FinancialStatus.Unpaid,
                LogisticStatus.Quote => reservation.FinancialStatus == FinancialStatus.Unpaid,
                _ => false
            };
        }
        private static bool CanTransitionFromQuoteB2B(LogisticStatus next, Reservation reservation)
        {
            return next switch
            {
                LogisticStatus.Cancelled => reservation.FinancialStatus == FinancialStatus.Unpaid,
                LogisticStatus.Confirmed => reservation.FinancialStatus == FinancialStatus.Unpaid,
                _ => false
            };
        }
        private static bool CanTransitionFromConfirmedB2B(LogisticStatus next, Reservation reservation)
        {
            return next switch
            {
                LogisticStatus.Cancelled => AllowedFinancialStatusFromConfirmedB2B.Contains(reservation.FinancialStatus),
                LogisticStatus.PickedUp => AllowedFinancialStatusFromConfirmedB2B.Contains(reservation.FinancialStatus),
                _ => false
            };
        }
        private static bool CanTransitionFromPickedUpB2B(LogisticStatus next, Reservation reservation)
        {
            return next == LogisticStatus.Returned && AllowedFinancialStatusFromConfirmedB2B.Contains(reservation.FinancialStatus);
        }
        private static bool CanTransitionFromReturnedB2B(LogisticStatus next, Reservation reservation)
        {
            return next == LogisticStatus.Checked && AllowedFinancialStatusFromConfirmedB2B.Contains(reservation.FinancialStatus);
        }
        private static bool CanTransitionFromCheckedB2B(LogisticStatus next, Reservation reservation)
        {
            return next switch
            {
                LogisticStatus.Damaged => AllowedFinancialStatusFromConfirmedB2B.Contains(reservation.FinancialStatus),
                LogisticStatus.Finished => reservation.FinancialStatus == FinancialStatus.Paid,
                _ => false
            };
        }
        private static bool CanTransitionFromDamagedB2B(LogisticStatus next, Reservation reservation)
        {
            return next == LogisticStatus.Finished && reservation.FinancialStatus == FinancialStatus.Paid;
        }
        private static bool CanTransitionFromCancelledB2B(LogisticStatus next, Reservation reservation)
        {   
            return next == LogisticStatus.Draft && 
                (reservation.FinancialStatus == FinancialStatus.Unpaid || reservation.FinancialStatus == FinancialStatus.Refunded);
        }        
        #endregion
    }
}