using CommuteMate.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardResponse> GetAsync(int userId, CancellationToken ct);
    }
}
