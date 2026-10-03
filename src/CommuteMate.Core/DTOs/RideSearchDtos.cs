using CommuteMate.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommuteMate.Core.DTOs
{
    public class SearchRidesRequest
    {
        [Required]
        public string FromCity { get; set; } = string.Empty;
        [Required]
        public string ToCity { get; set; } = string.Empty;
        [Required]
        public DateOnly? RideDate { get; set; }
        public decimal? MaxPricePerSeat { get; set; }
        public VehicleType? VehicleType { get; set; }
    }

    //we only reaveal phone number to the user who booked the ride, so we don't include it in the response
    public class SearchRidesResponse
    {
        public int RideId { get; set; }
        public string FromCity { get; set; } = string.Empty;
        public string ToCity { get; set; } = string.Empty;
        public string FromArea { get; set; } = string.Empty;
        public string ToArea { get; set; } = string.Empty;
        public DateOnly RideDate { get; set; }
        public TimeOnly RideTime { get; set; }
        public int AvailableSeats { get; set; }
        public decimal PricePerSeat { get; set; }
        public bool isRecurring { get; set; }
        public string Note { get; set; } = string.Empty;

        //publisher ifo =so the user can contact the publisher
        public string PublisherName { get; set; } = string.Empty;
        public decimal publisherRating { get; set; } 
        public int publishertotalrides
        {
            get; set;
        }
        //vehicle info
        public string VehicleModel { get; set; } = string.Empty;
        public string VehicleColor { get; set; } = string.Empty;
        public string vehicletype { get; set; } = string.Empty;
    }


}
