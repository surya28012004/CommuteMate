using System;
using Microsoft.EntityFrameworkCore;
using CommuteMate.Core.Entities;

namespace CommuteMate.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Ride> Rides => Set<Ride>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<CityDistance> CityDistances => Set<CityDistance>();
        public DbSet<UserActivitySummary> UserActivitySummaries => Set<UserActivitySummary>();
        public DbSet<UserRecentActivity> UserRecentActivities => Set<UserRecentActivity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
