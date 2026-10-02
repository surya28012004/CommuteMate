using CommuteMate.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommuteMate.Core.DTOs
{
    public class CreateVehicleRequest
    {
        [Required]
        public VehicleType Type { get; set; }
        [Required,MaxLength(100)]
        public string Make { get; set; }
        [Required,MaxLength(100)]
        public string Model { get; set; }
        [Required,MaxLength(50)]
        public string Color { get; set; }
        [Required,MaxLength(20)]
        public string? PlateNumber { get; set; }
        [Range(1,6,ErrorMessage = "Number of seats must be between 1 and 6.")]
        public int totalSeats { get; set; }


    }
    public class Vehicleresponse
    {
        public int Id { get; set; }
        public string Type { get; set; }= string.Empty;
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string? PlateNumber { get; set; } = string.Empty;
        public int totalSeats { get; set; }
    }
}
