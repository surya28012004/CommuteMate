using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommuteMate.Core.Entities;

namespace CommuteMate.Data.Configurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.ToTable("Vehicles");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Type).IsRequired();
            builder.Property(v => v.Make).IsRequired().HasMaxLength(100);
            builder.Property(v => v.Model).IsRequired().HasMaxLength(100);
            builder.Property(v => v.Color).HasMaxLength(50);
            builder.Property(v => v.PlateNumber).IsRequired().HasMaxLength(50);
            builder.Property(v => v.TotalSeats).IsRequired();
            builder.Property(v => v.IsActive).HasDefaultValue(true);
            builder.Property(v => v.CreatedAtUtc).IsRequired();

            builder.HasIndex(v => v.PlateNumber).IsUnique().HasDatabaseName("UQ_Vehicles_platenumber");

            // Prevent multiple cascade paths (User -> Vehicles -> Rides and User -> PublishedRides)
            // Use Restrict/NoAction so deleting a user won't cascade-delete vehicles (handle in app logic)
            builder.HasOne(v => v.User).WithMany(u => u.Vehicles).HasForeignKey(v => v.UserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
