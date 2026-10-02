using CommuteMate.Core.Entities;
using CommuteMate.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommuteMate.Data.Repositories
{
    public class VehicleRepository : IVehicleRepository

    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Vehicle>> GetActiveforuserAsync(int userId, CancellationToken ct)
        {
            return await _context.Vehicles
                .AsNoTracking()
                .Where(v => v.UserId == userId && v.IsActive == true).OrderBy(v => v.Id).ToListAsync(ct);
        }
        public async Task<bool> PlateExistsAsync(string plateNumber, CancellationToken ct)
        {
            return await _context.Vehicles.AnyAsync(v => v.PlateNumber == plateNumber && v.IsActive, ct);
        }

        public async Task<Vehicle?> getbyidforuserAsync(int vehicleId, int userId, CancellationToken ct)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId && v.UserId == userId && v.IsActive == true, ct);
        }
        public async Task AddAsync(Vehicle vehicle, CancellationToken ct)
        {
            //implement the logic to add a vehicle to the database
            await _context.Vehicles.AddAsync(vehicle, ct);
            
        }
        public async Task<int> SaveChangesAsync(CancellationToken ct)

        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
