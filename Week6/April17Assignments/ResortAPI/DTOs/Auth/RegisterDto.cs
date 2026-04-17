using System.ComponentModel.DataAnnotations;

namespace OnlineResortBooking.DTOs.Auth
{
    public class RegisterDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? MobileNumber { get; set; }

        // Optional role passed by client; default to "Customer" when not provided.
        public string Role { get; set; } = "Customer";
    }
}
