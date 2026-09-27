using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CommuteMate.Core.Entities;

namespace CommuteMate.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Token).IsRequired();
            builder.Property(rt => rt.ExpiryDateUtc).IsRequired();
            builder.Property(rt => rt.CreatedAtUtc).IsRequired();
            builder.Property(rt => rt.RevokedAtUtc).IsRequired(false);
            // Unique token lookup
            builder.HasIndex(rt => rt.Token).IsUnique().HasDatabaseName("UQ_RefreshTokens_Token");

            // Index to quickly find active (non-revoked) tokens for a user. Using a filtered index for SQL Server.
            builder.HasIndex(rt => new { rt.UserId, rt.RevokedAtUtc })
                   .HasDatabaseName("IX_RefreshTokens_UserId_RevokedAtUtc")
                   .HasFilter("[RevokedAtUtc] IS NULL");

            builder.HasOne(rt => rt.User).WithMany().HasForeignKey(rt => rt.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
