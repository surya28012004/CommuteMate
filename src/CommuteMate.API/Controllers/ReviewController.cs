using CommuteMate.API.Extensions;
using CommuteMate.Core.DTOs;
using CommuteMate.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommuteMate.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPost]
        public async Task<ActionResult<ReviewResponse>> Create(
            [FromBody] CreateReviewRequest request,
            CancellationToken ct)
        {
            var result = await _reviewService.CreateAsync(request, User.GetUserId(), ct);
            return Ok(result);
        }

    }
}
