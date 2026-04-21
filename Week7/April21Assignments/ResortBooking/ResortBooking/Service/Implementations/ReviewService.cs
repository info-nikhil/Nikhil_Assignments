using Microsoft.EntityFrameworkCore;
using OnlineResortBooking.API.Models;
using OnlineResortBooking.API.Services.Interfaces;

namespace OnlineResortBooking.API.Services.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            review.DateCreated = DateTime.Now;
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<List<Review>> GetReviewsByUserIdAsync(long userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}