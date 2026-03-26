using AceRental.Domain.Entities;
using AceRental.Domain.Enum;

namespace AceRental.Domain.Extensions
{
    public static class ReservationStatusExtensions
    {
        // --- CYCLE LOGISTIQUE ---
        // public static bool CanTransitionTo(this LogisticStatus current, LogisticStatus next, Reservation reservation)
        // {
        //     return current switch
        //     {
        //         // Entrée par le Commercial
        //         LogisticStatus.Draft => CanTransitionFromDraft(next, reservation),
        //         LogisticStatus.Quote => CanTransitionFromQuote(next, reservation),
        //         // Entrée par le Web
        //         LogisticStatus.Basket => CanTransitionFromBasket(next, reservation),
        //         // Cycle de vie actif
        //         LogisticStatus.Confirmed => CanTransitionFromConfirmed(next, reservation),
        //         LogisticStatus.PickedUp => CanTransitionFromPickedUp(next, reservation),
        //         // Cycle de clôture (Contrôle Qualité)
        //         LogisticStatus.Returned => CanTransitionFromReturned(next, reservation),
        //         LogisticStatus.Checked => CanTransitionFromChecked(next, reservation),
        //         // États finaux ou de sortie
        //         LogisticStatus.Cancelled => CanTransitionFromCancelled(next, reservation),
        //         LogisticStatus.Finished => false,
        //         LogisticStatus.Deleted => false,
        //         _ => false
        //     };
        // }
        // public static bool CanTransitionTo(this LogisticStatus current, FinancialStatus next, Reservation reservation)
        // {
        //     return current switch
        //     {
        //         LogisticStatus.Confirmed => CanTransitionFromConfirmed(next, reservation),
        //         LogisticStatus.Checked => CanTransitionFromChecked(next, reservation),
        //         _ => false
        //     };
        // }

        // // // --- CYCLE FINANCIER ---
        // public static bool CanTransitionTo(this FinancialStatus current, FinancialStatus next, Reservation reservation)
        // {
        //     return current switch
        //     {
        //         FinancialStatus.Unpaid => CanTransitionFromUnpaid(next, reservation),
        //         FinancialStatus.PartiallyPaid => CanTransitionFromPartiallyPaid(next, reservation),
        //         FinancialStatus.Paid => CanTransitionFromPaid(next, reservation),
        //         FinancialStatus.Invoiced => CanTransitionFromInvoiced(next, reservation),
        //         _ => false
        //     };
        // }
        // public static bool CanTransitionTo(this FinancialStatus current, LogisticStatus next, Reservation reservation)
        // {
        //     return current switch
        //     {
        //         FinancialStatus.Paid => CanTransitionFromPaid(next, reservation),
        //         FinancialStatus.Invoiced => CanTransitionFromInvoiced(next, reservation),
        //         _ => false
        //     };
        // }
        // private static bool CanTransitionFromDraft(LogisticStatus next, Reservation reservation) =>
        //     reservation.FinancialStatus == FinancialStatus.Unpaid && reservation.Workflow == Workflow.B2B &&
        //     (next == LogisticStatus.Quote || next == LogisticStatus.Deleted || next == LogisticStatus.Confirmed);

        // private static bool CanTransitionFromQuote(LogisticStatus next, Reservation reservation) =>
        //     reservation.FinancialStatus == FinancialStatus.Unpaid && reservation.Workflow == Workflow.B2B &&
        //     (next == LogisticStatus.Confirmed || next == LogisticStatus.Cancelled);

        // private static bool CanTransitionFromBasket(LogisticStatus next, Reservation reservation) =>
        //     reservation.FinancialStatus == FinancialStatus.Unpaid && reservation.Workflow == Workflow.B2C &&
        //     (next == LogisticStatus.Confirmed || next == LogisticStatus.Cancelled || next == LogisticStatus.Deleted);

        // private static bool CanTransitionFromConfirmed(LogisticStatus next, Reservation reservation) =>
        //     reservation.Workflow switch
        //     {
        //         Workflow.B2C =>
        //             (reservation.FinancialStatus == FinancialStatus.Unpaid && next == LogisticStatus.Cancelled) ||
        //             (reservation.FinancialStatus == FinancialStatus.Paid && next == LogisticStatus.PickedUp),
        //         Workflow.B2B =>
        //             (reservation.FinancialStatus == FinancialStatus.Unpaid && next == LogisticStatus.Cancelled) ||
        //             (reservation.FinancialStatus == FinancialStatus.PartiallyPaid && next == LogisticStatus.PickedUp),
        //         _ => false
        //     };
        // private static bool CanTransitionFromPickedUp(LogisticStatus next, Reservation reservation) =>
        //     reservation.Workflow switch
        //     {
        //         Workflow.B2C =>
        //             reservation.FinancialStatus == FinancialStatus.Paid && next == LogisticStatus.Returned,
        //         Workflow.B2B =>
        //             next == LogisticStatus.Returned,
        //         _ => false
        //     };

        // private static bool CanTransitionFromReturned(LogisticStatus next, Reservation reservation) =>
        //     reservation.Workflow switch
        //     {
        //         Workflow.B2C =>
        //             reservation.FinancialStatus == FinancialStatus.Paid && next == LogisticStatus.Checked,
        //         Workflow.B2B =>
        //             next == LogisticStatus.Checked,
        //         _ => false
        //     };

        // private static bool CanTransitionFromConfirmed(FinancialStatus next, Reservation reservation) =>

        //     reservation.Workflow == Workflow.B2C &&
        //     reservation.FinancialStatus == FinancialStatus.Unpaid &&
        //     next == FinancialStatus.Paid;

        // private static bool CanTransitionFromChecked(LogisticStatus next, Reservation reservation) =>

        //     reservation.Workflow switch
        //     {
        //         Workflow.B2C =>
        //             reservation.FinancialStatus == FinancialStatus.Invoiced && next == LogisticStatus.Finished,
        //         Workflow.B2B =>
        //             reservation.FinancialStatus == FinancialStatus.Paid && next == LogisticStatus.Finished,
        //         _ => false
        //     };

        // private static bool CanTransitionFromChecked(FinancialStatus next, Reservation reservation) =>
        //     reservation.Workflow == Workflow.B2B && next == FinancialStatus.Invoiced;

        // private static bool CanTransitionFromCancelled(LogisticStatus next, Reservation reservation) =>
        //     reservation.Workflow switch
        //     {
        //         Workflow.B2C =>
        //             reservation.FinancialStatus == FinancialStatus.Unpaid && next == LogisticStatus.Basket ||
        //             reservation.FinancialStatus == FinancialStatus.Unpaid && next == LogisticStatus.Deleted,
        //         Workflow.B2B =>
        //             reservation.FinancialStatus == FinancialStatus.Unpaid && next == LogisticStatus.Draft ||
        //             reservation.FinancialStatus == FinancialStatus.PartiallyPaid && next == LogisticStatus.Draft ||
        //             reservation.FinancialStatus == FinancialStatus.Unpaid && next == LogisticStatus.Deleted,
        //         _ => false
        //     };



        // private static bool CanTransitionFromUnpaid(FinancialStatus next, Reservation reservation) =>
        //     reservation.Workflow switch
        //     {
        //         Workflow.B2C => next == FinancialStatus.Paid && reservation.LogisticStatus == LogisticStatus.Confirmed,
        //         Workflow.B2B => CanTransitionFromUnpaidB2B(next, reservation),
        //         _ => false
        //     };
        // private static bool CanTransitionFromUnpaidB2B(FinancialStatus next, Reservation reservation) =>
        //     next switch
        //     {
        //         FinancialStatus.Invoiced => reservation.LogisticStatus == LogisticStatus.Checked,
        //         FinancialStatus.PartiallyPaid => reservation.LogisticStatus == LogisticStatus.Confirmed,
        //         _ => false
        //     };
        // private static bool CanTransitionFromPartiallyPaidB2B(FinancialStatus next, Reservation reservation) =>

        //     next switch
        //     {
        //         FinancialStatus.Paid => reservation.LogisticStatus == LogisticStatus.Checked,
        //         FinancialStatus.Invoiced => reservation.LogisticStatus == LogisticStatus.Confirmed,
        //         FinancialStatus.PartiallyPaid => reservation.LogisticStatus == LogisticStatus.Confirmed,
        //         _ => false
        //     };

        // private static bool CanTransitionFromPartiallyPaid(FinancialStatus next, Reservation reservation) =>
        // reservation.Workflow switch
        //     {
        //         Workflow.B2C => false, // Pas de transition autorisée depuis PartiallyPaid en B2C (exige paiement complet)
        //         Workflow.B2B => CanTransitionFromPartiallyPaidB2B(next, reservation),
        //         _ => false
        //     };

        // private static bool CanTransitionFromPaid(FinancialStatus next, Reservation reservation) =>
        //     reservation.Workflow switch
        //     {
        //         Workflow.B2C => next == FinancialStatus.Invoiced,
        //         Workflow.B2B => next == FinancialStatus.Refunded,
        //         _ => false
        //     };
        // private static bool CanTransitionFromPaid(LogisticStatus next, Reservation reservation) =>
        //     reservation.Workflow == Workflow.B2B && next == LogisticStatus.Finished;




        // private static bool CanTransitionFromInvoiced(FinancialStatus next, Reservation reservation) =>
        //     next == FinancialStatus.Refunded ||
        //     (reservation.Workflow == Workflow.B2B && next == FinancialStatus.Paid);

        // private static bool CanTransitionFromInvoiced(LogisticStatus next, Reservation reservation) =>
        //     reservation.Workflow == Workflow.B2C && next == LogisticStatus.PickedUp;
    }
}