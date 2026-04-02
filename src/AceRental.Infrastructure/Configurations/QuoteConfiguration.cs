using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class QuoteConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("Quotes");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.HasIndex(i => i.QuoteNumber)
            .IsUnique();

            builder.Property(ri => ri.TotalHT)
            .HasPrecision(18, 2);
            builder.Property(ri => ri.TVA)
            .HasPrecision(2, 2);
            builder.Property(ri => ri.TotalTTC)
            .HasPrecision(18, 2);
        }
    }
}
