using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int GiverId { get; set; }
        public int ReceiverId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public Booking Booking { get; set; } = null!;
    }
}
