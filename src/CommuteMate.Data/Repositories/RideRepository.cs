using CommuteMate.Core.DTOs;
using CommuteMate.Core.Entities;
using CommuteMate.Core.Enums;
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
        public async Task CompleteRideAsync(int rideId, CancellationToken ct)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(ct);
            var passengerIds= await _context.Bookings.Where(b => b.RideId == rideId && b.Status == BookingStatus.Confirmed)
                .Select(b => b.PassengerId)
                .ToListAsync(ct);

            await _context.Rides
                .Where(r => r.Id == rideId)
                .ExecuteUpdateAsync(r => r.SetProperty(r => r.Status, RideStatus.Completed), ct);
            await _context.Bookings
                .Where(b => b.RideId == rideId && b.Status == BookingStatus.Confirmed)
                .ExecuteUpdateAsync(b => b.SetProperty(b => b.Status, BookingStatus.Completed), ct);
            var publisherId = await _context.Rides
                .Where(r => r.Id == rideId)
                .Select(r => r.PublisherId)
                .FirstOrDefaultAsync(ct);
            await _context.Users
                .Where(u => u.Id == publisherId)
                .ExecuteUpdateAsync(u => u.SetProperty(u => u.TotalRides, u => u.TotalRides + 1), ct);
            if(passengerIds.Count>0)
            {
                await _context.Users
                    .Where(u => passengerIds.Contains(u.Id))
                    .ExecuteUpdateAsync(u => u.SetProperty(u => u.TotalRides, u => u.TotalRides + 1), ct);
            }
            await transaction.CommitAsync(ct);
        }
    }
}
