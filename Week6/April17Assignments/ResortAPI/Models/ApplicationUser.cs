using Microsoft.AspNetCore.Identity;

namespace OnlineResortBooking.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? MobileNumber { get; set; }
        public string UserRole { get; set; } = "Customer";
    }
}