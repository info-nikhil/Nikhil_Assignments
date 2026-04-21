using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.API.Models;
using OnlineResortBooking.API.Services.Interfaces;

namespace OnlineResortBooking.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _service;

        public AuthController(IUserService service)
        {
            _service = service;
        }

        // POST: api/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid data");

            var user = new User
            {
                Email = dto.Email,
                Password = dto.Password,
                Username = dto.Username,
                MobileNumber = dto.MobileNumber,
                UserRole = dto.UserRole
            };

            try
            {
                var created = await _service.RegisterUserAsync(user);

                return StatusCode(201, new
                {
                    message = "Registered successfully",
                    userId = created.UserId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid login request");

            try
            {
                var user = await _service.GetUserByEmailAsync(dto.Email);

                // Verify hashed password
                if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                    return Unauthorized("Invalid credentials");

                var token = await _service.GenerateJwtTokenAsync(user);

                return Ok(new
                {
                    token = token,
                    role = user.UserRole,
                    userId = user.UserId
                });
            }
            catch
            {
                return Unauthorized("Invalid credentials");
            }
        }
    }
}