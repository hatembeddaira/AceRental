using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
{
    public class ReservationEquipmentsConfiguration : IEntityTypeConfiguration<ReservationEquipments>
    {
        public void Configure(EntityTypeBuilder<ReservationEquipments> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("ReservationEquipments");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(ri => ri.UnitPriceAtTimeOfBooking)
            .HasPrecision(18, 2);
            builder.HasKey(re => new { re.ReservationId, re.EquipmentId });

            // 1. Relation avec Reservation
            builder.HasOne(re => re.Reservation)
                .WithMany(r => r.Equipments) 
                .HasForeignKey(re => re.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            // 2. Relation avec Equipment
            builder.HasOne(re => re.Equipment)
                .WithMany(r => r.Reservations)
                .HasForeignKey(re => re.EquipmentId)
                .OnDelete(DeleteBehavior.Cascade); 

        }
    }
}