using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("Invoices");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(ri => ri.AmountHT)
            .HasPrecision(18, 2);
            builder.HasIndex(i => i.InvoiceNumber)
            .IsUnique();


            // Relation 1-N : Une réservation peut avoir plusieurs factures
            builder.HasOne(p => p.Reservation)
                .WithMany(i => i.Invoices)
                .HasForeignKey(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation 1-N : Une facture peut avoir plusieurs paiements
            builder.HasMany(i => i.Payments)
                .WithOne(p => p.Invoice)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
