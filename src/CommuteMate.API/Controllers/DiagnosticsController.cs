using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CommuteMate.Core.Interfaces;
using CommuteMate.Data;

namespace CommuteMate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public DiagnosticsController(AppDbContext db)
        {
            _db = db;
        }

        // Simple endpoint to check DB connectivity
        [HttpGet("status")]
        public async Task<IActionResult> GetStatus(CancellationToken cancellationToken)
        {
            var canConnect = await _db.Database.CanConnectAsync(cancellationToken).ConfigureAwait(false);
            return Ok(new { DatabaseReachable = canConnect });
        }
    }
}
