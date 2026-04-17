using OnlineResortBooking.Models;

namespace OnlineResortBooking.Repository
{
    public interface IBookingRepository : IRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsByUserAsync(string userId);
        Task<IEnumerable<Booking>> GetAllWithDetailsAsync();
        Task<Booking?> GetByIdWithDetailsAsync(int id);
    }
}
