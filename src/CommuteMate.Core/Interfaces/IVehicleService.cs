using CommuteMate.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IVehicleService
    {
        Task<Vehicleresponse> AddAsync(CreateVehicleRequest request, int userId, CancellationToken ct);
        Task<List<Vehicleresponse>> GetMyVehiclesForUserAsync(int userId, CancellationToken ct);
        Task DeactivateAsync(int vehicleId, int userId, CancellationToken ct);
        
    }
}
