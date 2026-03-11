using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Paprec.SIRH.RevueRem.DataLayer.Configurations
{
    public class PackConfiguration : IEntityTypeConfiguration<Pack>
    {
        public void Configure(EntityTypeBuilder<Pack> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("Packs");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.HasIndex(i => i.Reference)
            .IsUnique();            
            builder.HasKey(pi => new { pi.Id });


            // Relation 1 → N avec PackItem
            builder.HasMany(r => r.Items)
                .WithOne(i => i.Pack)
                .HasForeignKey(i => i.PackId)
                .OnDelete(DeleteBehavior.Cascade); // supprime les items si pack supprimée
        }
    }
}
