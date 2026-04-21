using OnlineResortBooking.API.Models;

namespace OnlineResortBooking.API.Services.Interfaces
{
    public interface IBookingService
    {
        Task<Booking> GetBookingByIdAsync(long id);
        Task<List<Booking>> GetBookingsByUserIdAsync(long userId);
        Task<List<Booking>> GetAllBookingsAsync();
        Task<Booking> AddBookingAsync(Booking booking);
        Task<bool> DeleteBookingAsync(long id);
        Task<Booking> UpdateBookingStatusAsync(long id, string newStatus);
    }
}