using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommuteMate.Core.Entities;

namespace CommuteMate.Data.Configurations
{
    public class RideConfiguration : IEntityTypeConfiguration<Ride>
    {
        public void Configure(EntityTypeBuilder<Ride> builder)
        {
            builder.ToTable("Rides");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.FromCity).IsRequired().HasMaxLength(200);
            builder.Property(r => r.ToCity).IsRequired().HasMaxLength(200);
            builder.Property(r => r.FromArea).HasMaxLength(200);
            builder.Property(r => r.ToArea).HasMaxLength(200);
            builder.Property(r => r.RideDate).IsRequired();
            builder.Property(r => r.RideTime).IsRequired();
            builder.Property(r => r.AvailableSeats).IsRequired();
            builder.Property(r => r.PricePerSeat).HasColumnType("decimal(10,2)");
            builder.Property(r => r.Status).IsRequired();
            builder.Property(r => r.IsRecurring).HasDefaultValue(false);
            builder.Property(r => r.Notes).HasMaxLength(1000);
            builder.Property(r => r.CreatedAtUtc).IsRequired();

            // Index to optimize queries for a publisher's offered rides in dashboard
            builder.HasIndex(r => new { r.PublisherId, r.Status }).HasDatabaseName("IX_Rides_PublisherId_Status");

            // Avoid cascade cycles: restrict deletes and handle removals in application logic
            builder.HasOne(r => r.Publisher).WithMany(u => u.PublishedRides).HasForeignKey(r => r.PublisherId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(r => r.Vehicle).WithMany(v => v.Rides).HasForeignKey(r => r.VehicleId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
