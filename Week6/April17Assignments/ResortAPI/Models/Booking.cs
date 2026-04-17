using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineResortBooking.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public int NoOfPersons { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public string? Address { get; set; }

        public string Status { get; set; } = "Pending";

        // Navigation
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public int ResortId { get; set; }
        public Resort? Resort { get; set; }
    }
}