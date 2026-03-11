using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Paprec.SIRH.RevueRem.DataLayer.Configurations
{
    public class ReservationItemConfiguration : IEntityTypeConfiguration<ReservationItem>
    {
        public void Configure(EntityTypeBuilder<ReservationItem> builder)
        {
            if (builder is null)
            {
                return;
            }

            builder.ToTable("ReservationItems");
            builder.HasQueryFilter(e => !e.IsDeleted);
            builder.Property(ri => ri.UnitPriceAtTimeOfBooking)
            .HasPrecision(18, 2);
            
            // builder.HasOne(pi => pi.Reservation)
            //     .WithMany()
            //     .HasForeignKey(pi => pi.ReservationId)
            //     .OnDelete(DeleteBehavior.Cascade);
            
            // builder.HasOne(ri => ri.Equipment)
            //     .WithMany() 
            //     .HasForeignKey(ri => ri.EquipmentId)
            //     .OnDelete(DeleteBehavior.Cascade);

            // builder.HasOne(ri => ri.Pack)
            //     .WithMany()
            //     .HasForeignKey(ri => ri.PackId)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
