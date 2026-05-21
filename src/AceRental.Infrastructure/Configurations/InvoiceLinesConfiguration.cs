using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class InvoiceLinesConfiguration : IEntityTypeConfiguration<InvoiceLines>
    {
        public void Configure(EntityTypeBuilder<InvoiceLines> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("InvoiceLines");
            builder.Property(ri => ri.DailyPriceHT)
            .HasPrecision(18, 2);
            builder.Property(p => p.Type)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(pi => pi.Invoice)
            .WithMany(p => p.InvoiceLines) 
            .HasForeignKey(pi => pi.InvoiceId);
        }
    }
}
