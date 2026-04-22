using System.ComponentModel.DataAnnotations;

namespace ResortBookingUI.MVC.Models
{
    public class Resort
    {
        public long ResortId { get; set; }

        [Required]
        public string ResortName { get; set; }

        public string ResortImageUrl { get; set; }

        [Required]
        public string ResortLocation { get; set; }

        public string ResortAvailableStatus { get; set; }

        [Required]
        public long Price { get; set; }

        [Required]
        public int Capacity { get; set; }

        public string Description { get; set; }
    }
}