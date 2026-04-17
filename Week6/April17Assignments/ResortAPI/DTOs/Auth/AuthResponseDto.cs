namespace OnlineResortBooking.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiry { get; set; }
        public string? Role { get; set; }
    }
}