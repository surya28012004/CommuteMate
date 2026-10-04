using CommuteMate.Core.DTOs;
using CommuteMate.Core.Entities;
using CommuteMate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Services
{
    public class DashboardService : IDashboardService
    {
        private const int RecentActivityCount = 10;

        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardResponse> GetAsync(int userId, CancellationToken ct)
        {
            var summary = await _dashboardRepository.GetSummaryAsync(userId, ct);

            // The view filters IsActive = 1, so a deactivated user returns nothing.
            if (summary is null)
            {
                throw new Exception("User not found.");
            }

            var activity = await _dashboardRepository.GetRecentActivityAsync(userId, RecentActivityCount, ct);

            return new DashboardResponse
            {
                Summary = MapSummary(summary),
                RecentActivity = activity.Select(MapActivity).ToList()
            };
        }

        private static DashboardSummaryResponse MapSummary(UserActivitySummary s) => new()
        {
            FullName = s.FullName,
            AverageRating = s.AverageRating,
            TotalRides = s.TotalRides,
            RidesPublished = s.RidesPublished,
            ActiveRidesPublished = s.ActiveRidesPublished,
            CompletedRidesPublished = s.CompletedRidesPublished,
            BookingsMade = s.BookingsMade,
            UpcomingBookings = s.UpcomingBookings,
            CompletedBookings = s.CompletedBookings,
            TotalSavings = s.TotalSavings
        };

        private static RecentActivityResponse MapActivity(UserRecentActivity a) => new()
        {
            ActivityType = a.ActivityType,
            RideId = a.RideId,
            Route = $"{a.FromCity} to {a.ToCity}",
            RideDate = a.RideDate,
            RideTime = a.RideTime,
            Status = a.StatusText,
            Amount = a.Amount,
            ActivityAtUtc = a.ActivityAtUtc
        };
    }
}
