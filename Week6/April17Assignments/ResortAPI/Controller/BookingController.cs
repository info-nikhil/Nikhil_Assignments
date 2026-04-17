using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.DTOs.Booking;
using OnlineResortBooking.Service;

namespace OnlineResortBooking.Controller
{
    [ApiController]
    [Route("api/booking")]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var items = await _bookingService.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            var isAdmin = User.IsInRole("Admin");

            var booking = await _bookingService.GetByIdAsync(id);
            if (booking == null) return NotFound();

            if (!isAdmin && booking.UserId != userId) return Forbid();
            return Ok(booking);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var caller = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && caller != userId) return Forbid();

            var items = await _bookingService.GetByUserAsync(userId);
            return Ok(items);
        }

        [HttpPost]
        [Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> Create([FromBody] BookingDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            var created = await _bookingService.CreateAsync(dto, userId ?? throw new InvalidOperationException());
            return CreatedAtAction(nameof(Get), new { id = created.BookingId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookingDto dto)
        {
            var caller = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            var isAdmin = User.IsInRole("Admin");

            var ok = await _bookingService.UpdateAsync(id, dto, caller ?? "", isAdmin);
            if (!ok) return Forbid();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var caller = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            var isAdmin = User.IsInRole("Admin");

            var ok = await _bookingService.DeleteAsync(id, caller ?? "", isAdmin);
            if (!ok) return Forbid();
            return NoContent();
        }
    }
}
