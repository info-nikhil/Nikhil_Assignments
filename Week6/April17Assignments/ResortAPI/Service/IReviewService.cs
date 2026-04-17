using OnlineResortBooking.DTOs.Review;

namespace OnlineResortBooking.Service
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetAllAsync();
        Task<IEnumerable<ReviewDto>> GetByUserAsync(string userId);
        Task<ReviewDto> CreateAsync(ReviewDto dto, string userId);
    }
}
