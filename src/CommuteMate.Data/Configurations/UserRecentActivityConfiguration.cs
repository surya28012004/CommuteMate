using CommuteMate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Data.Configurations
{
    public class UserRecentActivityConfiguration : IEntityTypeConfiguration<UserRecentActivity>
    {
        public void Configure(EntityTypeBuilder<UserRecentActivity> builder)
        {
            builder.HasNoKey();
            builder.ToView(null);        // procedure result — not a table, not a view

            builder.Property(x => x.ActivityType).HasMaxLength(20);
            builder.Property(x => x.FromCity).HasMaxLength(100);
            builder.Property(x => x.ToCity).HasMaxLength(100);
            builder.Property(x => x.StatusText).HasMaxLength(20);
            builder.Property(x => x.Amount).HasPrecision(10, 2);
        }
    }
}
