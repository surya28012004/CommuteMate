using CommuteMate.Core.DTOs;
using CommuteMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IRideRepository
    {
        Task<List<Ride>> GetByPublisherAsync(int publisherId, CancellationToken ct);
        Task AddAsync(Ride ride, CancellationToken ct);
        Task<int> SaveChangesAsync(CancellationToken ct);
        //don't show the searcher their own rides in the search results
        Task<List<Ride>> SearchRidesAsync(SearchRidesRequest request,int excludeUserId, CancellationToken ct);
        Task<Ride?> GetByIdAsync(int rideId, CancellationToken ct);

        Task CompleteRideAsync(int rideId, CancellationToken ct);
    }
}
