using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Entities;

public class UserRecentActivity
{
    public string ActivityType { get; set; } = string.Empty;   // "Published" | "Booked"
    public int RideId { get; set; }
    public string FromCity { get; set; } = string.Empty;
    public string ToCity { get; set; } = string.Empty;
    public DateOnly RideDate { get; set; }
    public TimeOnly RideTime { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime ActivityAtUtc { get; set; }
}

