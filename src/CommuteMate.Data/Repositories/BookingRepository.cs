using CommuteMate.Core.Entities;
using CommuteMate.Core.Enums;
using CommuteMate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Data.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> TryCreateBookingAsync(Booking booking, CancellationToken ct)
        {
            //await using auto rollback if anything throws
            await using var transaction = await _context.Database.BeginTransactionAsync(ct);
            var rowsUpdated=await _context.Rides.Where(r => r.Id == booking.RideId && r.AvailableSeats >= booking.SeatsBooked && r.Status == RideStatus.Active)
                .ExecuteUpdateAsync(r => r.SetProperty(x => x.AvailableSeats, x => x.AvailableSeats - booking.SeatsBooked), ct);
            if(rowsUpdated == 0)
            {
                await transaction.RollbackAsync(ct);
                return false;
            }
            await _context.Bookings.AddAsync(booking, ct);
            await _context.SaveChangesAsync(ct);
            await _context.Rides.
                Where(r => r.Id == booking.RideId && r.AvailableSeats == 0)
                .ExecuteUpdateAsync(r => r.SetProperty(x => x.Status, x => x.AvailableSeats == 0 ? RideStatus.Full : x.Status), ct);

            await transaction.CommitAsync(ct);
            return true;
        }
        public async Task CancelBookingAsync(Booking booking, CancellationToken ct)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(ct);
            booking.Status = BookingStatus.Cancelled;
            await _context.SaveChangesAsync(ct);

            await _context.Rides.Where(r => r.Id == booking.RideId)
                //add also if ridestatus is full, then set it to active

                .ExecuteUpdateAsync(r => r.SetProperty(x => x.AvailableSeats, x => x.AvailableSeats + booking.SeatsBooked)
                .SetProperty(x => x.Status,r=> r.Status == RideStatus.Full ? RideStatus.Active : r.Status), ct);
            await transaction.CommitAsync(ct);
        }

        public Task<List<Booking>> GetBookingsAsync(int passengerId, CancellationToken ct)
        {
            return _context.Bookings.AsNoTracking()
                .Include(b => b.Ride)
                .ThenInclude(r => r.Publisher)
                .Include(b => b.Ride)
                .ThenInclude(r => r.Vehicle)
                .Where(b => b.PassengerId == passengerId)
                .OrderByDescending(b => b.BookedAtUtc)
                .ToListAsync(ct);
        }

        public async Task<Booking?> GetByIdForUserAsync(int bookingId, int passengerId, CancellationToken ct)
        {
            return await _context.Bookings.Include(b=> b.Ride)
           
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.PassengerId == passengerId, ct);
        }

        public async Task<bool> HasActiveBookingAsync(int rideId, int passengerId, CancellationToken ct)
        {
            return await _context.Bookings.AsNoTracking()
                .AnyAsync(b => b.RideId == rideId && b.PassengerId == passengerId && b.Status == BookingStatus.Confirmed, ct);
        }

        
    }
}
