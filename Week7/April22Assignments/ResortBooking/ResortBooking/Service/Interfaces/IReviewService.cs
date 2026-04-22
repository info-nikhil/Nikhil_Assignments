using OnlineResortBooking.API.Models;

namespace OnlineResortBooking.API.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<Review>> GetAllReviewsAsync();
        Task<Review> AddReviewAsync(Review review);
        Task<List<Review>> GetReviewsByUserIdAsync(long userId);
    }
}