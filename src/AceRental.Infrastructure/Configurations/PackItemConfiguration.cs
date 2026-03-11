using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Paprec.SIRH.RevueRem.DataLayer.Configurations
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
            .WithMany(e => e.PackItems) // L'entité Equipment doit avoir ICollection<PackItem> PackItems
            .HasForeignKey(pi => pi.EquipmentId);

        }
    }
}
