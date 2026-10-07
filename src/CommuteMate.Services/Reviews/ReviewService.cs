using CommuteMate.Core.DTOs;
using CommuteMate.Core.Entities;
using CommuteMate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Services.Reviews
{
    public class ReviewService:IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }
        public async Task<ReviewResponse> CreateAsync(CreateReviewRequest request, int giverID, CancellationToken ct)
        {
            var Booking = await _reviewRepository.GetCompletedBookingAsync(request.bookingId, ct);
            if (Booking == null)
            {
                throw new InvalidOperationException("Booking not found or not completed.");
            }
            int receiverId;
            if (Booking.PassengerId == giverID)
            {
                receiverId = Booking.Ride.PublisherId;
            }
            else if (Booking.Ride.PublisherId == giverID)
            {
                receiverId = Booking.PassengerId;
            }
            else
            {
                throw new InvalidOperationException("Giver is not part of the booking.");
            }
            var review = new Review
            {
                BookingId = Booking.Id,
                GiverId = giverID,
                ReceiverId = receiverId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAtUtc = DateTime.UtcNow
            };
            var result = await _reviewRepository.AddReviewAndUpdateAverageAsync(review, ct);
            if (!result)
            {
                throw new InvalidOperationException("Failed to add review or update average rating.");
            }
            return new ReviewResponse
            {
                Id = review.Id,
                BookingId = review.BookingId,
                giverid = review.GiverId,
                receiverid = review.ReceiverId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAtUtc = review.CreatedAtUtc
            };
        }

        }
}
