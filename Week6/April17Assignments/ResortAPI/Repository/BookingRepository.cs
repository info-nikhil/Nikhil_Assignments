using Microsoft.EntityFrameworkCore;
using OnlineResortBooking.Data;
using OnlineResortBooking.Models;


namespace OnlineResortBooking.Repository
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId)
        {
            return await _dbSet
                .Where(r => r.UserId == userId)
                .Include(r => r.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllWithUsersAsync()
        {
            return await _dbSet
                .Include(r => r.User)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
