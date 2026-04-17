namespace OnlineResortBooking.DTOs.Resort
{
    public class ResortDto
    {
        public int? ResortId { get; set; }
        public string ResortName { get; set; } = null!;
        public string? ResortImageUrl { get; set; }
        public string? ResortLocation { get; set; }
        public bool ResortAvailableStatus { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }
    }
}