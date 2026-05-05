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
            
            // Relation 1 → N avec ReservationEquipments
            builder.HasMany(r => r.Equipments)
                .WithOne(i => i.Reservation)
                .HasForeignKey(i => i.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // supprime les Equipments si réservation supprimée
            
            // Relation 1 → N avec ReservationPacks
            builder.HasMany(r => r.Packs)
                .WithOne(i => i.Reservation)
                .HasForeignKey(i => i.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // supprime les Packs si réservation supprimée
            
            // Relation 1 → N avec ReservationServices
            builder.HasMany(r => r.Services)
                .WithOne(i => i.Reservation)
                .HasForeignKey(i => i.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // supprime les Services si réservation supprimée

            // Relation 1 → N avec Quote
            builder.HasMany(r => r.Quotes)
                .WithOne(q => q.Reservation)
                .HasForeignKey(q => q.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // supprime les quotes si réservation supprimée

            // Relation 1 → N avec Invoice
            builder.HasMany(r => r.Invoices)
                .WithOne(i => i.Reservation)
                .HasForeignKey(i => i.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // supprime les invoices si réservation supprimée

            // Relation 1 → N avec Payment
            builder.HasMany(r => r.Payments)
                .WithOne(p => p.Reservation)
                .HasForeignKey(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // supprime les payments si réservation supprimée

            // Relation N → 1 avec Client
            builder.HasOne(r => r.Client)
                .WithMany(c => c.Reservations) 
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Cascade); // ne pas supprimer client si réservation existe
                    
        }
    }
}
