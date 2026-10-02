using CommuteMate.API.Extensions;
using CommuteMate.Core.DTOs;
using CommuteMate.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommuteMate.API.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    [Authorize]
    public class RideController: ControllerBase

    {
        private readonly IRideService _rideService;
        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRide([FromBody] CreateRideRequest request, CancellationToken ct)
        {
            var result = await _rideService.CreateRideAsync(request, User.GetUserId(), ct);
            return Ok(result);
        }
        [HttpGet("my-rides")]
        public async Task<IActionResult> GetMyRides(CancellationToken ct)
        {
            var rides = await _rideService.GetMyRidesAsync(User.GetUserId(), ct);
            return Ok(rides);
        }

    }
}
