using CommuteMate.Core.Enums;

namespace CommuteMate.Core.Entities
{
   

    public class Ride
    {
        public int Id { get; set; }

        public int PublisherId { get; set; }
        public int VehicleId { get; set; }

        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;

        public string? FromArea { get; set; }
        public string? ToArea { get; set; }

        public DateOnly RideDate { get; set; }
        public TimeOnly RideTime { get; set; }

        public int AvailableSeats { get; set; }
        public decimal PricePerSeat { get; set; }

        public RideStatus Status { get; set; } = RideStatus.Active;

        public bool IsRecurring { get; set; }
        public string? RecurringDays { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public User Publisher { get; set; } = null!;
        public Vehicle Vehicle { get; set; } = null!;

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}