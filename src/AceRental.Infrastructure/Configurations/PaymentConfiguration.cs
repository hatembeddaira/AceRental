using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class PaymentConfiguration: IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");

            // builder.HasKey(p => p.Id);

            // Précision monétaire obligatoire pour éviter les arrondis SQL
            builder.Property(p => p.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.Date)
                .IsRequired();

            builder.Property(p => p.TransactionId)
                .HasMaxLength(100);

            // Configuration des Enums en chaînes de caractères dans la DB (plus lisible)
            builder.Property(p => p.Type)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(p => p.Method)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Relation N → 1 avec Reservation
            builder.HasOne(p => p.Reservation)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation N → 1 avec Invoice (optionnel)
            builder.HasOne(p => p.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}