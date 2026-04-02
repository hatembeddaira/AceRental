using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
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
            builder.Property(ri => ri.TotalHT)
            .HasPrecision(18, 2);
            builder.Property(ri => ri.TVA)
            .HasPrecision(2, 2);
            builder.Property(ri => ri.TotalTTC)
            .HasPrecision(18, 2);
            builder.HasIndex(i => i.ReservationNumber)
            .IsUnique();

            builder.Property(p => p.FinancialStatus)
                .HasConversion<string>()
                .HasMaxLength(20);
            builder.Property(p => p.LogisticStatus)
                .HasConversion<string>()
                .HasMaxLength(20);
            builder.Property(p => p.Workflow)
                .HasConversion<string>()
                .HasMaxLength(20);
            
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
