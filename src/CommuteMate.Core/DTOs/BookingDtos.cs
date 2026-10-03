using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommuteMate.Core.DTOs
{
    public class CreateBookingRequest
    {
        [Required]
        public int RideId { get; set; }

        public int SeatsBooked { get; set; }
    }
    public class BookingResponse
    {
        public int BookingId { get; set; }
        public int RideId { get; set; }
        public int SeatsBooked { get; set; }
        public DateTime BookingAtUtc { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }

        //Ride details
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public DateOnly RideDate { get; set; }
        public TimeOnly RideTime { get; set; }

        //Publisher info ,phone number is included here so the user can contact the publisher
        public string PublisherName { get; set; } = string.Empty;
        public string PublisherPhone { get; set; } = string.Empty;
         
    }

}

