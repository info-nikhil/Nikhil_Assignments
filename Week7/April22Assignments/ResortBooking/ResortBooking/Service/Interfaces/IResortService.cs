using OnlineResortBooking.API.Models;

namespace OnlineResortBooking.API.Services.Interfaces
{
    public interface IResortService
    {
        Task<List<Resort>> GetAllResortsAsync();
        Task<Resort> GetResortByIdAsync(long id);
        Task<Resort> AddResortAsync(Resort resort);
        Task<Resort> UpdateResortAsync(long id, Resort resort);
        Task<bool> DeleteResortAsync(long id);
    }
}