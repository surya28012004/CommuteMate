using CommuteMate.Core.Enums;

namespace CommuteMate.Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int RideId { get; set; }
        public int PassengerId { get; set; }
        public int SeatsBooked { get; set; } = 1;
        public decimal TotalPrice { get; set; }
        public decimal DiscountAppliedPercent { get; set; }
        public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
        public DateTime BookedAtUtc { get; set; } = DateTime.UtcNow;
        public Ride Ride { get; set; } = null!;
        public User Passenger { get; set; } = null!;
    }
}