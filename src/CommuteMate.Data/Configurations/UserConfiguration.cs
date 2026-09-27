using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommuteMate.Core.Entities;

namespace CommuteMate.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FullName).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            builder.Property(u => u.Phone).IsRequired().HasMaxLength(50);
            builder.Property(u => u.PasswordHash).IsRequired();
            // Precision 2,1 allows values from 0.0 to 9.9 (e.g., 4.5). Add a check constraint to enforce range.
            builder.Property(u => u.AverageRating).HasPrecision(2, 1).HasDefaultValue(0.0m);
            builder.Property(u => u.TotalRides).HasDefaultValue(0);
            builder.Property(u => u.CreatedAtUtc).IsRequired();
            builder.Property(u => u.IsActive).HasDefaultValue(true);
            builder.HasIndex(u => u.Email).IsUnique().HasDatabaseName("UQ_Users_Email");
            builder.HasIndex(u => u.Phone).IsUnique().HasDatabaseName("UQ_Users_Phone");
        }
    }
}
