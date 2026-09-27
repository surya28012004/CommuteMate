using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public decimal AverageRating { get; set; } = 0.0m;
        public int TotalRides { get; set; } = 0;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<Ride> PublishedRides { get; set; } = new List<Ride>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
