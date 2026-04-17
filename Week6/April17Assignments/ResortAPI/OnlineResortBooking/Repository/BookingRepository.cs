using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineResortBooking.Data;
using OnlineResortBooking.Models;

namespace OnlineResortBooking.Repository
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.Resort)
                .Include(b => b.User)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllWithDetailsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Resort)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Resort)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }
    }
}
