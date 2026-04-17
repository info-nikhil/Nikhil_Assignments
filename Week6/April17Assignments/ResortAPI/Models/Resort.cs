using System.ComponentModel.DataAnnotations;

namespace OnlineResortBooking.Models
{
    public class Resort
    {
        public int ResortId { get; set; }

        [Required]
        public string ResortName { get; set; } = null!;

        public string? ResortImageUrl { get; set; }

        public string? ResortLocation { get; set; }

        public bool ResortAvailableStatus { get; set; } = true;

        public decimal Price { get; set; }

        public int Capacity { get; set; }

        public string? Description { get; set; }

        public ICollection<Booking>? Bookings { get; set; }

        public ICollection<Review>? Reviews { get; set; }
    }
}