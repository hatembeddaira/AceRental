using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class PackItemConfiguration : IEntityTypeConfiguration<PackItem>
    {
        public void Configure(EntityTypeBuilder<PackItem> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("PackItems");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.HasKey(pi => new { pi.EquipmentId, pi.PackId });

            builder.HasOne(pi => pi.Equipment)
            .WithMany(e => e.PackItems) 
            .HasForeignKey(pi => pi.EquipmentId);

            builder.HasOne(pi => pi.Pack)
            .WithMany(p => p.Items) 
            .HasForeignKey(pi => pi.PackId);

        }
    }
}
