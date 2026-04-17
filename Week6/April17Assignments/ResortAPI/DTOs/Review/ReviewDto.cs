namespace OnlineResortBooking.DTOs.Review
{
    public class ReviewDto
    {
        public int? ReviewId { get; set; }
        public string? UserId { get; set; }
        public string Subject { get; set; } = null!;
        public string? Body { get; set; }
        public int Rating { get; set; }
    }
}
