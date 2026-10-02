using CommuteMate.Core.Entities;
using CommuteMate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Data.Repositories
{
    public class RideRepository : IRideRepository
    {
        private readonly AppDbContext _context;
        public RideRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Ride ride, CancellationToken ct)
        {
            await _context.Rides.AddAsync(ride, ct);
           
        }

        public Task<List<Ride>> GetByPublisherAsync(int publisherId, CancellationToken ct)
        {
            return _context.Rides.
                AsNoTracking()
                .Include(r=>r.Vehicle)
                .Where(r => r.PublisherId == publisherId)
                .OrderByDescending(r => r.RideDate)
                .ThenByDescending(r => r.RideTime)
                .ToListAsync(ct);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return await _context.SaveChangesAsync(ct);
           
        }
    }
}
