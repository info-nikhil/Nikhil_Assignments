using OnlineResortBooking.API.Models;

namespace OnlineResortBooking.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(User user);
        Task<string> GenerateJwtTokenAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(long userId);
    }
}