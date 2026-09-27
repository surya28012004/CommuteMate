using System;

namespace CommuteMate.Core.Entities
{
    public class CityDistance
    {
        public int Id { get; set; }
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public decimal DistanceKm { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
