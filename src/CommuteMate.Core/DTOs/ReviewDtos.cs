using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommuteMate.Core.DTOs
{
    public class CreateReviewRequest
    {
        [Range(1,5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string Comment { get; set; } = string.Empty;
        public int bookingId { get; set; }

    }
    public class ReviewResponse 
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int giverid { get; set; }
        public int receiverid { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAtUtc { get; set; }

    }

}
