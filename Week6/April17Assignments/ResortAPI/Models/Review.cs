using System.ComponentModel.DataAnnotations;

namespace OnlineResortBooking.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required]
        public string Subject { get; set; } = null!;

        public string? Body { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}