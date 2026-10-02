using CommuteMate.Core.DTOs;
using CommuteMate.Core.Entities;
using CommuteMate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CommuteMate.Services.Rides
{
    public class RideService : IRideService
    {
        private readonly IRideRepository _rideRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public RideService(IRideRepository rideRepository, IVehicleRepository vehicleRepository)
        {
            _rideRepository = rideRepository ?? throw new ArgumentNullException(nameof(rideRepository));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        public async Task<RideResponse> CreateRideAsync(CreateRideRequest request, int userId, CancellationToken ct)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var vehicle = await _vehicleRepository.getbyidforuserAsync(request.VehicleId, userId, ct);
            if (vehicle == null)
                throw new InvalidOperationException("Vehicle not found or does not belong to the user.");

            if (!vehicle.IsActive)
                throw new InvalidOperationException("Cannot create a ride with an inactive vehicle.");

            
            if (request.AvailableSeats > vehicle.TotalSeats)
                throw new InvalidOperationException("Available seats cannot exceed vehicle total seats.");

            var rideStart = request.RideDate.ToDateTime(request.RideTime);
            if (rideStart < DateTime.UtcNow)
                throw new InvalidOperationException("Ride date and time cannot be in the past.");

            if (request.isRecurring && string.IsNullOrWhiteSpace(request.RecurringDays))
                throw new InvalidOperationException("Recurring days must be specified for recurring rides.");

            var ride = new Ride
            {
                PublisherId = userId,
                VehicleId = request.VehicleId,
                FromCity = request.Fromcity?.Trim() ?? string.Empty,
                ToCity = request.Tocity?.Trim() ?? string.Empty,
                FromArea = string.IsNullOrWhiteSpace(request.FromArea) ? null : request.FromArea.Trim(),
                ToArea = string.IsNullOrWhiteSpace(request.ToArea) ? null : request.ToArea.Trim(),
                RideDate = request.RideDate,
                RideTime = request.RideTime,
                AvailableSeats = request.AvailableSeats,
                PricePerSeat = request.PricePerSeat,
                IsRecurring = request.isRecurring,
                RecurringDays = string.IsNullOrWhiteSpace(request.RecurringDays) ? null : request.RecurringDays.Trim(),
                Notes = string.IsNullOrWhiteSpace(request.Note) ? null : request.Note.Trim(),
                CreatedAtUtc = DateTime.UtcNow,
                Status = Core.Enums.RideStatus.Active
            };

            await _rideRepository.AddAsync(ride, ct);
            await _rideRepository.SaveChangesAsync(ct);
            ride.Vehicle = vehicle; // Assign the vehicle to the ride for mapping purposes
            return MapToResponse(ride);
        }

        public async Task<List<RideResponse>> GetMyRidesAsync(int userId, CancellationToken ct)
        {
            var rides = await _rideRepository.GetByPublisherAsync(userId, ct);
            return rides.Select(MapToResponse).ToList();
        }

        private static RideResponse MapToResponse(Ride ride)
        {
            return new RideResponse
            {
                VehicleId = ride.VehicleId,
                Fromcity = ride.FromCity,
                Tocity = ride.ToCity,
                FromArea = ride.FromArea ?? string.Empty,
                ToArea = ride.ToArea ?? string.Empty,
                RideDate = ride.RideDate,
                RideTime = ride.RideTime,
                AvailableSeats = ride.AvailableSeats,
                PricePerSeat = ride.PricePerSeat,
                isRecurring = ride.IsRecurring,
                RecurringDays = ride.RecurringDays,
                Note = ride.Notes ?? string.Empty
            };
        }
    }
}
