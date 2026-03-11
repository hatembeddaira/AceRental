using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Paprec.SIRH.RevueRem.DataLayer.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("Reservations");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(ri => ri.TotalAmount)
            .HasPrecision(18, 2);
            builder.HasIndex(i => i.ReservationNumber)
            .IsUnique();
            
            // Relation 1 → N avec ReservationItem
            builder.HasMany(r => r.Items)
                .WithOne(i => i.Reservation)
                .HasForeignKey(i => i.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // supprime les items si réservation supprimée

            // Relation N → 1 avec Client
            builder.HasOne(r => r.Client)
                .WithMany(c => c.Reservations) 
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict); // ne pas supprimer client si réservation existe
                    
        }
    }
}
