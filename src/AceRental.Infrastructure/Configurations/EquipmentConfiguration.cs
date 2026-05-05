using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("Equipments");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(ri => ri.DailyPriceHT)
            .HasPrecision(18, 2);
            builder.Property(ri => ri.PurchasePriceTTC)
            .HasPrecision(18, 2);
            builder.Property(ri => ri.NewPurchasePriceTTC)
            .HasPrecision(18, 2);
            builder.HasIndex(i => i.Reference)
            .IsUnique();
            builder.Property(p => p.Category)
                .HasConversion<string>()
                .HasMaxLength(20);

        
        }
    }
}