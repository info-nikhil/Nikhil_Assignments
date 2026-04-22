namespace OnlineResortBooking.API.Models;

public class Review
{
    public int ReviewId { get; set; }
    public long UserId { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public int Rating { get; set; }
    public DateTime DateCreated { get; set; }

    public long BookingId { get; set; }
    public long ResortId { get; set; }

    public User? User { get; set; }
    public Resort? Resort { get; set; }
}