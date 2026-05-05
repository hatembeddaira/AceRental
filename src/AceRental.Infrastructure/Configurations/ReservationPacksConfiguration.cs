using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class ReservationPacksConfiguration : IEntityTypeConfiguration<ReservationPacks>
    {
        public void Configure(EntityTypeBuilder<ReservationPacks> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("ReservationPacks");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(ri => ri.UnitPriceAtTimeOfBooking)
            .HasPrecision(18, 2);
            builder.HasKey(re =>  new { re.ReservationId, re.PackId }); // Clé composite

            // 1. Relation avec Reservation
            builder.HasOne(re => re.Reservation)
                .WithMany(r => r.Packs) 
                .HasForeignKey(re => re.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Relation avec Pack
            builder.HasOne(re => re.Pack)
                .WithMany(p=> p.Reservations)
                .HasForeignKey(re => re.PackId)
                .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}