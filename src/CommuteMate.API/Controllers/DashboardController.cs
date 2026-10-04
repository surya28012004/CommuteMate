using CommuteMate.API.Extensions;
using CommuteMate.Core.DTOs;
using CommuteMate.Core.Entities;
using CommuteMate.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommuteMate.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<DashboardResponse>> Get(CancellationToken ct)
        {
            var result = await _dashboardService.GetAsync(User.GetUserId(), ct);
            return Ok(result);
        }
    }
}
