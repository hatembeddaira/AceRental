using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class QuoteLinesConfiguration : IEntityTypeConfiguration<QuoteLines>
    {
        public void Configure(EntityTypeBuilder<QuoteLines> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("QuoteLines");
            builder.Property(ri => ri.DailyPriceHT)
            .HasPrecision(18, 2);
            builder.Property(p => p.Type)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasOne(pi => pi.Quote)
            .WithMany(p => p.QuoteLines) 
            .HasForeignKey(pi => pi.QuoteId);
        }
    }
}
