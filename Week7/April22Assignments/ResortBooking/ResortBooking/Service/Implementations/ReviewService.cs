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
                .Include(r => r.Resort)
                .ToListAsync();
        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            // VALIDATION
            var userExists = await _context.Users.AnyAsync(u => u.UserId == review.UserId);
            var bookingExists = await _context.Bookings.AnyAsync(b => b.BookingId == review.BookingId);
            var resortExists = await _context.Resorts.AnyAsync(r => r.ResortId == review.ResortId);

            if (!userExists)
                throw new Exception("Invalid UserId");

            if (!bookingExists)
                throw new Exception("Invalid BookingId");

            if (!resortExists)
                throw new Exception("Invalid ResortId");

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