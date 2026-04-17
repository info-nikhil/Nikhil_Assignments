using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.DTOs.Review;
using OnlineResortBooking.Service;

namespace OnlineResortBooking.Controller
{
    [ApiController]
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var items = await _reviewService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var items = await _reviewService.GetByUserAsync(userId);
            return Ok(items);
        }

        [HttpPost]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> Create([FromBody] ReviewDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            var created = await _reviewService.CreateAsync(dto, userId ?? throw new InvalidOperationException());
            return CreatedAtAction(nameof(GetByUser), new { userId = created.UserId }, created);
        }
    }
}
