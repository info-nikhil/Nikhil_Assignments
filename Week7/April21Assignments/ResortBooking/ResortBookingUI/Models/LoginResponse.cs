namespace ResortBookingUI.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public long UserId { get; set; }
    }
}