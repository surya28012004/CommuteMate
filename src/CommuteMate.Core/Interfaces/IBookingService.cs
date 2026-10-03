using CommuteMate.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponse> CreateAsync(CreateBookingRequest request, int userId, CancellationToken ct);
        Task CancelAsync(int bookingId, int userId, CancellationToken ct);
        Task<List<BookingResponse>> GetMyBookingsAsync(int userId, CancellationToken ct);
    }
}
