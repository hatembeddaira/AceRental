using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("Services");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(ri => ri.DailyPriceHT)
            .HasPrecision(18, 2);
            // Configuration des Enums en chaînes de caractères dans la DB (plus lisible)
            builder.Property(p => p.Type)
                .HasConversion<string>()
                .HasMaxLength(20);
        }
    }
}