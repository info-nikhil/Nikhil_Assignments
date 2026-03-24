using System.ComponentModel.DataAnnotations;

namespace CodeFIrst_EF_in_ASPNET.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your age")]
        [Range(0, 100, ErrorMessage = "Please enter between 0 and 100")]
        public int Age { get; set; }
    }
}
