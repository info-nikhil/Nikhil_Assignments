using System.ComponentModel.DataAnnotations;

namespace ResortBookingUI.MVC.Models
{
    public class Booking
    {
        public long BookingId { get; set; }

        [Required]
        public int NoOfPersons { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public string Address { get; set; }

        public string Status { get; set; } // Pending / Accepted / Rejected

        public long UserId { get; set; }

        [Required]
        public long ResortId { get; set; }

        public Resort Resort { get; set; }
    }
}