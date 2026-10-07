using CommuteMate.Core.Entities;
using CommuteMate.Core.Enums;
using CommuteMate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CommuteMate.Data.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;
        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<Booking?> GetCompletedBookingAsync(int bookingId, CancellationToken ct)
        {
            return _context.Bookings
                .AsNoTracking()
                .Include(b => b.Ride)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.Status == BookingStatus.Completed, ct);
        }
        public async Task<bool> AddReviewAndUpdateAverageAsync(Review review, CancellationToken ct)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, ct);
            try
            {
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync(ct);
                var averageRating = await _context.Reviews
                    .Where(r => r.ReceiverId == review.ReceiverId)
                    .AverageAsync(r => r.Rating, ct);
                var receiver = await _context.Users.SingleOrDefaultAsync(u => u.Id == review.ReceiverId, ct);
                receiver.AverageRating = (decimal)averageRating;
                await _context.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return true;

            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
    }
}
