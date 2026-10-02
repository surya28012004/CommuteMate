using CommuteMate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Core.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> getbyidforuserAsync(int vehicleId, int userId, CancellationToken ct);
        Task<List<Vehicle>> GetActiveforuserAsync(int userId, CancellationToken ct);
        Task<bool> PlateExistsAsync(string plateNumber, CancellationToken ct);
        Task AddAsync(Vehicle vehicle, CancellationToken ct);
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
