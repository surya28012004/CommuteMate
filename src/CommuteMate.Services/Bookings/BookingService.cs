using CommuteMate.Core.DTOs;
using CommuteMate.Core.Entities;
using CommuteMate.Core.Enums;
using CommuteMate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Services.Bookings
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRideRepository _rideRepository;
        public BookingService(IBookingRepository bookingRepository, IRideRepository rideRepository)
        {
            _bookingRepository = bookingRepository;
            _rideRepository = rideRepository;
        }
       

        public async Task<BookingResponse> CreateAsync(CreateBookingRequest request, int userId, CancellationToken ct)
        {
            // Check if the ride exists
            var ride = await _rideRepository.GetByIdAsync(request.RideId, ct);
            if (ride == null)
            {
                throw new Exception("Ride not found.");
            }
            if(ride.PublisherId == userId)
            {
                throw new Exception("You cannot book your own ride.");
            }if(ride.Status!=RideStatus.Active)
            {
                throw new Exception("You cannot book a ride that is not active.");
            }
            if(ride.RideDate.ToDateTime(ride.RideTime) < DateTime.UtcNow)
            {
                throw new Exception("You cannot book a ride that has already started.");
            }
            var booking = new Booking
            {
                RideId = ride.Id,
                PassengerId = userId,
                SeatsBooked = request.SeatsBooked,
                TotalPrice = request.SeatsBooked * ride.PricePerSeat,
                DiscountAppliedPercent = 0, // Assuming no discount logic for now
                BookedAtUtc = DateTime.UtcNow,
                Status = BookingStatus.Confirmed

            };
            var success=await _bookingRepository.TryCreateBookingAsync(booking, ct);
            if (!success)
            {
                throw new Exception("Sorry these seats are not enough available.please try another ride");
            }
            booking.Ride = ride; // Include ride details in the response
            return MapToRespose(booking);

        }

        public async Task<List<BookingResponse>> GetMyBookingsAsync(int userId, CancellationToken ct)
        {
            var bookings = await _bookingRepository.GetBookingsAsync(userId, ct);
            return bookings.Select(b => MapToRespose(b)).ToList();
        }
        public async Task CancelAsync(int bookingId, int userId, CancellationToken ct)
        {
            var booking = await _bookingRepository.GetByIdForUserAsync(bookingId, userId, ct);
            if(booking == null)
            {
                throw new Exception("Booking not found or you are not authorized to cancel this booking.");
            }
            if(booking.Status != BookingStatus.Confirmed)
            {
                throw new Exception("Only confirmed bookings can be canceled.");
            }
            //Business rule: no cancellation at thr last minute
            var ridestart=booking.Ride.RideDate.ToDateTime(booking.Ride.RideTime);
            if (ridestart <= DateTime.UtcNow.AddHours(1))
            {
                throw new Exception("You cannot cancel a booking less than 1 hour before the ride starts.");
            }
            await _bookingRepository.CancelBookingAsync(booking, ct);
        }

        private static BookingResponse MapToRespose(Booking b)
        {
            return new BookingResponse
            {
                BookingId = b.Id,
                SeatsBooked = b.SeatsBooked,
                TotalPrice = b.TotalPrice,
                Status = b.Status.ToString(),
                BookingAtUtc = b.BookedAtUtc,

                FromCity = b.Ride.FromCity,
                ToCity = b.Ride.ToCity,
                RideDate = b.Ride.RideDate,
                RideTime = b.Ride.RideTime,
                PublisherName = b.Ride.Publisher.FullName,
                PublisherPhone = b.Ride.Publisher.Phone,
            };
        }
    }
}
