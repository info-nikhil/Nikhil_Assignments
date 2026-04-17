using Microsoft.EntityFrameworkCore;
using OnlineResortBooking.Data;
using OnlineResortBooking.Models;

namespace OnlineResortBooking.Repository
{
    public class ResortRepository : GenericRepository<Resort>, IResortRepository
    {
        public ResortRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Resort>> GetAvailableResortsAsync()
        {
            return await _dbSet.Where(r => r.ResortAvailableStatus).AsNoTracking().ToListAsync();
        }
    }
}