using CommuteMate.Core.DTOs;
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
        public async Task<List<Ride>> SearchRidesAsync(SearchRidesRequest request, int excludeUserId, CancellationToken ct)
        {
            var query = _context.Rides
                .AsNoTracking()
                .Include(r => r.Publisher)
                .Include(r => r.Vehicle)
                .Where(r => r.FromCity==request.FromCity
                && r.ToCity == request.ToCity
                && r.PublisherId != excludeUserId
                && r.RideDate == request.RideDate
                && r.AvailableSeats > 0 );
            if(request.VehicleType.HasValue)
            {
                query = query.Where(r => r.Vehicle.Type == request.VehicleType.Value);
            }
            if(request.MaxPricePerSeat.HasValue)
            {
                query = query.Where(r => r.PricePerSeat <= request.MaxPricePerSeat.Value);
            }
            return await query
                .OrderByDescending(r => r.RideDate)
                .ThenByDescending(r => r.RideTime)
                .Take(50)
                .ToListAsync(ct);
        }
        public async Task<Ride?> GetByIdAsync(int rideId, CancellationToken ct)
        {
            return await _context.Rides
                .AsNoTracking()
                .Include(r => r.Publisher)
                .Include(r => r.Vehicle)
                .FirstOrDefaultAsync(r => r.Id == rideId, ct);
        }
    }
}
