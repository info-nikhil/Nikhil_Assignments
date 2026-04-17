using OnlineResortBooking.Models;

namespace OnlineResortBooking.Repository
{
    public interface IResortRepository : IRepository<Resort>
    {
        Task<IEnumerable<Resort>> GetAvailableResortsAsync();
    }
}