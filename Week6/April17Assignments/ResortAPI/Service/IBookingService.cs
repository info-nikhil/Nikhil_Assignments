using OnlineResortBooking.DTOs.Booking;

namespace OnlineResortBooking.Service
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingDto>> GetAllAsync();
        Task<BookingDto?> GetByIdAsync(int id);
        Task<IEnumerable<BookingDto>> GetByUserAsync(string userId);
        Task<BookingDto> CreateAsync(BookingDto dto, string userId);
        Task<bool> UpdateAsync(int id, BookingDto dto, string callerUserId, bool isAdmin);
        Task<bool> DeleteAsync(int id, string callerUserId, bool isAdmin);
    }
}
