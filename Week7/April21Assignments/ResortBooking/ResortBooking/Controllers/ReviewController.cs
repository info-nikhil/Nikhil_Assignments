using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.API.Models;
using OnlineResortBooking.API.Services.Interfaces;

namespace OnlineResortBooking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/review")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }

        // GET: api/review
        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _service.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // GET: api/review/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetReviewsByUserId(long userId)
        {
            var reviews = await _service.GetReviewsByUserIdAsync(userId);
            return Ok(reviews);
        }

        // POST: api/review
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            if (review == null)
                return BadRequest("Invalid review data");

            var created = await _service.AddReviewAsync(review);
            return StatusCode(201, created);
        }
    }
}