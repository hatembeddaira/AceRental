using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class ReservationServicesConfiguration : IEntityTypeConfiguration<ReservationServices>
    {
        public void Configure(EntityTypeBuilder<ReservationServices> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("ReservationServices");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(ri => ri.UnitPriceAtTimeOfBooking)
            .HasPrecision(18, 2);
            builder.HasKey(re =>  new { re.ReservationId, re.ServiceId });

            // 1. Relation avec Reservation
            builder.HasOne(re => re.Reservation)
                .WithMany(r => r.Services) 
                .HasForeignKey(re => re.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Relation avec Service
            builder.HasOne(re => re.Service)
                .WithMany(s => s.Reservations)
                .HasForeignKey(re => re.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}