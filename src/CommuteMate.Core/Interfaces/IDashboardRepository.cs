using CommuteMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IDashboardRepository
    {
        Task<UserActivitySummary?> GetSummaryAsync(int userId, CancellationToken ct);
        Task<List<UserRecentActivity>> GetRecentActivityAsync(int userId, int topN, CancellationToken ct);
    }
}
