using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommuteMate.Core.DTOs
{
    public class CreateRideRequest
    {
        [Required]
        public int VehicleId { get; set; }
        public string Fromcity { get; set; } = string.Empty;
        public string Tocity { get; set; } = string.Empty;
        public string FromArea { get; set; } = string.Empty;
        public string ToArea { get; set; } = string.Empty;
        [Required]
        public DateOnly RideDate { get; set; }
        public TimeOnly RideTime { get; set; }
        [Range(1, 6)]
        public int AvailableSeats { get; set; }
        public decimal PricePerSeat { get; set; }
        public bool isRecurring { get; set; }
        public string? RecurringDays { get; set; } // Comma-separated days of the week (e.g., "Monday,Wednesday,Friday")

        public string Note { get; set; } = string.Empty;
    }
    public class RideResponse
    {
        public int VehicleId { get; set; }
        public string Fromcity { get; set; } = string.Empty;
        public string Tocity { get; set; } = string.Empty;
        public string FromArea { get; set; } = string.Empty;
        public string ToArea { get; set; } = string.Empty;
        public DateOnly RideDate { get; set; }
        public TimeOnly RideTime { get; set; }
        public int AvailableSeats { get; set; }
        public decimal PricePerSeat { get; set; }
        public bool isRecurring { get; set; }
        public string? RecurringDays { get; set; } // Comma-separated days of the week (e.g., "Monday,Wednesday,Friday")
        public string Note { get; set; } = string.Empty;
    }
}
