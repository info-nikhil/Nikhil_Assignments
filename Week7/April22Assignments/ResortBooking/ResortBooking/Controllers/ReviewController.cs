using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.API.Models;
using OnlineResortBooking.API.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _service;

    public ReviewController(IReviewService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllReviewsAsync());
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUser(long userId)
    {
        return Ok(await _service.GetReviewsByUserIdAsync(userId));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Review review)
    {
        var result = await _service.AddReviewAsync(review);
        return Ok(result);
    }
}