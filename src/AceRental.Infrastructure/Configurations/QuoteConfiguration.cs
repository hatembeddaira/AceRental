using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Paprec.SIRH.RevueRem.DataLayer.Configurations
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
        }
    }
}
