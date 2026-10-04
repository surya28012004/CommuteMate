using CommuteMate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Data.Configurations
{
    public class UserActivitySummaryConfiguration : IEntityTypeConfiguration<UserActivitySummary>
    {
        public void Configure(EntityTypeBuilder<UserActivitySummary> builder)
        {
            builder.HasNoKey();                              // read-only, no PK
            builder.ToView("vw_UserActivitySummary");        // EF reads, migrations ignore

            builder.Property(x => x.FullName).HasMaxLength(100);
            builder.Property(x => x.AverageRating).HasPrecision(2, 1);
            builder.Property(x => x.TotalSavings).HasPrecision(10, 2);
        }
    }
}
