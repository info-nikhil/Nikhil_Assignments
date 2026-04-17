using OnlineResortBooking.Models;

namespace OnlineResortBooking.Repository
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId);
        Task<IEnumerable<Review>> GetAllWithUsersAsync();
    }
}
