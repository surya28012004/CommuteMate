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
        //[FromQuery] tells ASP .NET Core to bind the query parameters from the URL to the properties of the SearchRidesRequest object. This allows you to pass search criteria as query parameters in the request URL, and they will be automatically mapped to the corresponding properties of the SearchRidesRequest object.
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchRidesRequest request, CancellationToken ct)
        {
            var rides = await _rideService.SearchAsync(request, User.GetUserId(), ct);
            return Ok(rides);
        }
        [HttpPut("{id:int}/complete")]
        public async Task<IActionResult> Complete(int id, CancellationToken ct)
        {
            await _rideService.CompleteAsync(id, User.GetUserId(), ct);
            return Ok(new { message = "Ride completed successfully." });
        }
    }
}
