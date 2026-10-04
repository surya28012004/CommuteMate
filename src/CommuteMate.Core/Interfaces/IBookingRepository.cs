using CommuteMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IBookingRepository
    {
        // <summary>
        // Tries to create a new booking in the database.

        Task<bool> TryCreateBookingAsync(Booking booking, CancellationToken ct);
        Task CancelBookingAsync(Booking booking, CancellationToken ct);

        Task<bool> HasActiveBookingAsync(int rideId, int passengerId, CancellationToken ct);
        Task<List<Booking>> GetBookingsAsync(int passengerId, CancellationToken ct);

        Task<Booking?> GetByIdForUserAsync(int bookingId,int passengerId, CancellationToken ct);
        Task<int> GetCompletedRideCountBetweenUsersAsync(int passengerId, int publisherId,CancellationToken ct);

        Task<decimal> GetDiscountPercentAsync(int passengerId,int publisherId,CancellationToken ct);
    }
}
