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
    }
}
