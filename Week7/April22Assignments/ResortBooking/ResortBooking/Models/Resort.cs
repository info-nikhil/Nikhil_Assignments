using System.Text.Json.Serialization;

namespace OnlineResortBooking.API.Models;

public class Resort
{
    public long ResortId { get; set; }
    public string ResortName { get; set; }
    public string ResortImageUrl { get; set; }
    public string ResortLocation { get; set; }
    public string ResortAvailableStatus { get; set; }
    public long Price { get; set; }
    public int Capacity { get; set; }
    public string Description { get; set; }

    [JsonIgnore]
    public ICollection<Booking>? Bookings { get; set; }
}