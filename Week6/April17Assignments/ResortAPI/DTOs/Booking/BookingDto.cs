namespace OnlineResortBooking.DTOs.Booking
{
    public class BookingDto
    {
        public int? BookingId { get; set; }
        public int NoOfPersons { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? Address { get; set; }
        public string? Status { get; set; }
        public string? UserId { get; set; }
        public int ResortId { get; set; }
    }
}   
