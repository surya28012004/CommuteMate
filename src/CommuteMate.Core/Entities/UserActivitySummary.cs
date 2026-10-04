using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Entities
{
    public class UserActivitySummary
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public decimal AverageRating { get; set; }
        public int TotalRides { get; set; }

        public int RidesPublished { get; set; }
        public int ActiveRidesPublished { get; set; }
        public int CompletedRidesPublished { get; set; }

        public int BookingsMade { get; set; }
        public int UpcomingBookings { get; set; }
        public int CompletedBookings { get; set; }

        public decimal TotalSavings { get; set; }
    }
}
