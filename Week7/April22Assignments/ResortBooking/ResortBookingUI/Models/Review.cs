namespace ResortBookingUI.MVC.Models
{
    public class Review
    {
        public long ReviewId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public int Rating { get; set; }

        public long UserId { get; set; }     // REQUIRED

        public long ResortId { get; set; }   // REQUIRED

        public long BookingId { get; set; }  // REQUIRED

        public User? User { get; set; }
        public Resort? Resort { get; set; }
    }
}