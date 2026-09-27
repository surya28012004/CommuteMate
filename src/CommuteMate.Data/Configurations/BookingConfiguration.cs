using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommuteMate.Core.Entities;

namespace CommuteMate.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.SeatsBooked).HasDefaultValue(1);
            builder.Property(b => b.TotalPrice).HasColumnType("decimal(10,2)");
            builder.Property(b => b.DiscountAppliedPercent).HasColumnType("decimal(5,2)");
            builder.Property(b => b.Status).IsRequired();
            builder.Property(b => b.BookedAtUtc).IsRequired();

            builder.HasOne(b => b.Ride).WithMany(r => r.Bookings).HasForeignKey(b => b.RideId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(b => b.Passenger).WithMany(u => u.Bookings).HasForeignKey(b => b.PassengerId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
