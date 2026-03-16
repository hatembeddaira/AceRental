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
        }
    }
}
