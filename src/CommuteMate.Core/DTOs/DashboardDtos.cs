using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.DTOs
{
    public class DashboardResponse
    {
        public DashboardSummaryResponse Summary { get; set; } = new();
        public List<RecentActivityResponse> RecentActivity { get; set; } = new();
    }

    public class DashboardSummaryResponse
    {
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
    public class RecentActivityResponse
    {
        public string ActivityType { get; set; } = string.Empty;
        public int RideId { get; set; }
        public string Route { get; set; } = string.Empty;
        public DateOnly RideDate { get; set; }
        public TimeOnly RideTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ActivityAtUtc { get; set; }
    }
}
