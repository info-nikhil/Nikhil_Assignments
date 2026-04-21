using System.ComponentModel.DataAnnotations;

namespace ResortBookingUI.MVC.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public long UserId { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime DateCreated { get; set; }
    }
}