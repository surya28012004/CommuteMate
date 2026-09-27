using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommuteMate.Core.Entities;

namespace CommuteMate.Data.Configurations
{
    public class CityDistanceConfiguration : IEntityTypeConfiguration<CityDistance>
    {
        public void Configure(EntityTypeBuilder<CityDistance> builder)
        {
            builder.ToTable("CityDistances");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FromCity).IsRequired().HasMaxLength(200);
            builder.Property(c => c.ToCity).IsRequired().HasMaxLength(200);
            builder.Property(c => c.DistanceKm).HasColumnType("decimal(8,2)");
            builder.Property(c => c.CreatedAtUtc).IsRequired();
        }
    }
}
