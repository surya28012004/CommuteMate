using CommuteMate.Core.DTOs;
using CommuteMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CommuteMate.Core.Interfaces
{
    public interface IRideService
    {
        Task<RideResponse> CreateRideAsync(CreateRideRequest request, int userId, CancellationToken ct);
        Task<List<RideResponse>> GetMyRidesAsync(int userId, CancellationToken ct);
        Task<List<SearchRidesResponse>> SearchAsync(SearchRidesRequest request, int userId, CancellationToken ct);
        Task CompleteAsync(int rideId, int userId, CancellationToken ct);
    }
}
