using System.ComponentModel.DataAnnotations;

namespace ResortBookingUI.MVC.Models
{
    public class User
    {
        public long UserId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }

        public string UserRole { get; set; } // Admin / Customer
    }
}