using AceRental.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AceRental.Infrastructure.Configurations
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
        }
    }
}
