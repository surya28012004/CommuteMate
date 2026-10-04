using CommuteMate.Core.Entities;
using CommuteMate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Data.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;
        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<UserActivitySummary?> GetSummaryAsync(int userId, CancellationToken ct)
        {
            return _context.UserActivitySummaries.FirstOrDefaultAsync(u => u.UserId == userId, ct);

        }
        public Task<List<UserRecentActivity>> GetRecentActivityAsync(int userId, int topN, CancellationToken ct)
        {
            return _context.Database.SqlQuery<UserRecentActivity>(
                $"EXEC dbo.sp_GetUserRecentActivity @UserId = {userId}, @TopN = {topN}")
                .ToListAsync(ct);
        }

    }
}
