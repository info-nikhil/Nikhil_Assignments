using Microsoft.EntityFrameworkCore;
using OnlineResortBooking.API.Models;
using OnlineResortBooking.API.Services.Interfaces;

namespace OnlineResortBooking.API.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= GET BY ID =================
        public async Task<Booking> GetBookingByIdAsync(long id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Resort)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                throw new Exception("Booking not found");

            return booking;
        }

        // ================= GET BY USER =================
        public async Task<List<Booking>> GetBookingsByUserIdAsync(long userId)
        {
            return await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Resort)
                .ToListAsync();
        }

        // ================= GET ALL =================
        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            return await _context.Bookings
                .Include(b => b.Resort)
                .Include(b => b.User)
                .ToListAsync();
        }

        // ================= ADD =================
        public async Task<Booking> AddBookingAsync(Booking booking)
        {
            //  Validate Resort exists
            var resort = await _context.Resorts.FindAsync(booking.ResortId);
            if (resort == null)
                throw new Exception("Invalid Resort");

            booking.Status = "Pending";

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            // Reload with Resort (IMPORTANT for UI)
            return await _context.Bookings
                .Include(b => b.Resort)
                .FirstOrDefaultAsync(b => b.BookingId == booking.BookingId);
        }

        // ================= DELETE =================
        public async Task<bool> DeleteBookingAsync(long id)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                throw new Exception("Booking not found");

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return true;
        }

        // ================= UPDATE STATUS =================
        public async Task<Booking> UpdateBookingStatusAsync(long id, string newStatus)
        {
            //  Validate status
            if (newStatus != "Accepted" && newStatus != "Rejected")
                throw new Exception("Invalid status. Use 'Accepted' or 'Rejected'");

            var booking = await _context.Bookings
                .Include(b => b.Resort)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                throw new Exception("Booking not found");

            booking.Status = newStatus;

            await _context.SaveChangesAsync();

            return booking;
        }
    }
}