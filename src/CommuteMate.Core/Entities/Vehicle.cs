using CommuteMate.Core.Enums;

namespace CommuteMate.Core.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public VehicleType Type { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string PlateNumber { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public User User { get; set; } = null!;

        public ICollection<Ride> Rides { get; set; } = new List<Ride>();
    }
}