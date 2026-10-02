using CommuteMate.API.Extensions;
using CommuteMate.Core.DTOs;
using CommuteMate.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CommuteMate.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateVehicleRequest request, CancellationToken ct)
        {
            var result = await _vehicleService.AddAsync(request,User.GetUserId(), ct);
            return Ok(result);
        }
        [HttpGet("my-vehicles")]
        public async Task<IActionResult> GetMyVehicles(CancellationToken ct)
        {
            var vehicles = await _vehicleService.GetMyVehiclesForUserAsync(User.GetUserId(), ct);
            return Ok(vehicles);
        }
        [HttpDelete("{vehicleId:int}")]
        public async Task<IActionResult> Delete(int vehicleId, CancellationToken ct)
        {
            await _vehicleService.DeactivateAsync(vehicleId, User.GetUserId(), ct);
            return Ok(new { message = "Vehicle deactivated successfully." });
        }
    }
}
