using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.API.Models;
using OnlineResortBooking.API.Services.Interfaces;

namespace OnlineResortBooking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        // GET: api/booking
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _service.GetAllBookingsAsync();
            return Ok(bookings);
        }

        // GET: api/booking/{bookingId}
        [HttpGet("{bookingId}")]
        public async Task<IActionResult> GetBooking(long bookingId)
        {
            var booking = await _service.GetBookingByIdAsync(bookingId);
            return Ok(booking);
        }

        // GET: api/booking/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsByUserId(long userId)
        {
            var bookings = await _service.GetBookingsByUserIdAsync(userId);
            return Ok(bookings);
        }

        // POST: api/booking
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] Booking booking)
        {
            if (booking == null)
                return BadRequest("Invalid booking data");

            var created = await _service.AddBookingAsync(booking);
            return StatusCode(201, created);
        }

        // PUT: api/booking/{bookingId}
        [Authorize(Roles = "Admin")]
        [HttpPut("{bookingId}")]
        public async Task<IActionResult> UpdateBookingStatus(long bookingId, [FromBody] string status)
        {
            // VALIDATION HERE
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status is required");

            if (status != "Accepted" && status != "Rejected")
                return BadRequest("Invalid status. Use 'Accepted' or 'Rejected'");

            var updated = await _service.UpdateBookingStatusAsync(bookingId, status);

            return Ok(updated);
        }

        // DELETE: api/booking/{bookingId}
        [HttpDelete("{bookingId}")]
        public async Task<IActionResult> DeleteBooking(long bookingId)
        {
            var result = await _service.DeleteBookingAsync(bookingId);
            return Ok(new { message = "Booking deleted successfully", result });
        }
    }
}