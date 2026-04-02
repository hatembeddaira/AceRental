using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AceRental.Application.Payments.Dtos;
using AceRental.Application.Reservations.Dtos;
using AceRental.Domain.Enum;

namespace AceRental.Application.Invoices.Dtos
{
    public class InvoiceDto
    {
        public Guid Id { get; set; }
        public required int InvoiceNumber { get; set; } = 0;
        public decimal AmountHT { get; set; }
        public decimal TVA { get; set; } = 0.20m;
        public decimal AmountTTC => AmountHT * (1 + TVA);
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public InvoiceType Type { get; set; }

        // Relation 1-to-1 avec la réservation
        public Guid ReservationId { get; set; }
        public ReservationDetailsDto Reservation { get; set; } = null!;
        
        // Relation 1-to-n avec la Paiement
        public Guid PaymentId { get; set; }
        public ICollection<PaymentDto> Payment { get; set; } = null!;
    }
}