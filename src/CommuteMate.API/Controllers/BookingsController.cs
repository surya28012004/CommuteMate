using CommuteMate.API.Extensions;
using CommuteMate.Core.DTOs;
using CommuteMate.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommuteMate.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class BookingsController:ControllerBase
    {
        private readonly IBookingService _bookingService;
        
        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request, CancellationToken ct)
        {
            var result = await _bookingService.CreateAsync(request, User.GetUserId(), ct);
            return Ok(result);
        }
        [HttpGet("my-bookings")]
        public async Task<IActionResult> GetMyBookings(CancellationToken ct)
        {
            var bookings = await _bookingService.GetMyBookingsAsync(User.GetUserId(), ct);
            return Ok(bookings);
        }
        [HttpPut("{bookingId}/cancel")]
        public async Task<IActionResult> CancelBooking(int bookingId, CancellationToken ct)
        {
            await _bookingService.CancelAsync(bookingId, User.GetUserId(), ct);
            return Ok(new { message = "Booking cancelled successfully." });
        }
    }
}
