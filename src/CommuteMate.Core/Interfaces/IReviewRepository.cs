using CommuteMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<Booking?> GetCompletedBookingAsync(int bookingId, CancellationToken ct);
        Task<bool> AddReviewAndUpdateAverageAsync(Review review, CancellationToken ct);
    }
}
