using CommuteMate.Core.DTOs;
using CommuteMate.Core.Entities;
using CommuteMate.Core.Enums;
using CommuteMate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommuteMate.Services.Vehicles
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Vehicleresponse> AddAsync(CreateVehicleRequest request, int userId, CancellationToken ct)
        {
            var plate = request.PlateNumber.Replace(" ", "").ToUpperInvariant();
            if (await _vehicleRepository.PlateExistsAsync(plate, ct))
            {
                throw new InvalidOperationException("A vehicle with the same plate number already exists.");
            }

            if (request.Type == VehicleType.car && request.totalSeats < 4)
            {
                throw new InvalidOperationException("A car must have at least 4 seats.");
            }

            if (request.Type == VehicleType.bike && request.totalSeats != 1)
            {
                throw new InvalidOperationException("A bike must have exactly 1 seat.");
            }

            var vehicle = new Vehicle
            {
                UserId = userId,
                Type = request.Type,
                Make = request.Make.Trim(),
                Model = request.Model.Trim(),
                Color = request.Color.Trim(),
                PlateNumber = plate,
                TotalSeats = request.totalSeats,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _vehicleRepository.AddAsync(vehicle, ct);
            await _vehicleRepository.SaveChangesAsync(ct);

            return MapToResponse(vehicle);
        }

        private Vehicleresponse MapToResponse(Vehicle vehicle)
        {
            return new Vehicleresponse
            {
                Id = vehicle.Id,
                Type = vehicle.Type.ToString(),
                Make = vehicle.Make,
                Model = vehicle.Model,
                Color = vehicle.Color,
                PlateNumber = vehicle.PlateNumber,
                totalSeats = vehicle.TotalSeats,
            };
        }

        async Task IVehicleService.DeactivateAsync(int vehicleId, int userId, CancellationToken ct)
        {
            var vehicle = await _vehicleRepository.getbyidforuserAsync(vehicleId, userId, ct);
            if (vehicle == null)
            {
                throw new InvalidOperationException("Vehicle not found or does not belong to the user.");
            }

            vehicle.IsActive = false;
            await _vehicleRepository.SaveChangesAsync(ct);
        }

        async Task<List<Vehicleresponse>> IVehicleService.GetMyVehiclesForUserAsync(int userId, CancellationToken ct)
        {
            var vehicles = await _vehicleRepository.GetActiveforuserAsync(userId, ct);
            return vehicles.Select(MapToResponse).ToList();
        }
    }
}