using OnlineResortBooking.DTOs.Review;
using OnlineResortBooking.Models;
using OnlineResortBooking.Repository;

namespace OnlineResortBooking.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<ReviewDto> CreateAsync(ReviewDto dto, string userId)
        {
            var review = new Review
            {
                Subject = dto.Subject,
                Body = dto.Body,
                Rating = dto.Rating,
                UserId = userId,
                DateCreated = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();

            dto.ReviewId = review.ReviewId;
            dto.UserId = review.UserId;
            return dto;
        }

        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var items = await _reviewRepository.GetAllWithUsersAsync();
            return items.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                Subject = r.Subject,
                Body = r.Body,
                Rating = r.Rating
            });
        }

        public async Task<IEnumerable<ReviewDto>> GetByUserAsync(string userId)
        {
            var items = await _reviewRepository.GetReviewsByUserAsync(userId);
            return items.Select(r => new ReviewDto
            {
                ReviewId = r.ReviewId,
                UserId = r.UserId,
                Subject = r.Subject,
                Body = r.Body,
                Rating = r.Rating
            });
        }
    }
}
