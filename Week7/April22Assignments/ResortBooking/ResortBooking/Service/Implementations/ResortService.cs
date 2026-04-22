using Microsoft.EntityFrameworkCore;
using OnlineResortBooking.API.Models;
using OnlineResortBooking.API.Services.Interfaces;

namespace OnlineResortBooking.API.Services.Implementations
{
    public class ResortService : IResortService
    {
        private readonly ApplicationDbContext _context;

        public ResortService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Resort>> GetAllResortsAsync()
        {
            return await _context.Resorts.ToListAsync();
        }

        public async Task<Resort> GetResortByIdAsync(long id)
        {
            var resort = await _context.Resorts.FindAsync(id);
            if (resort == null) throw new Exception("Resort not found");
            return resort;
        }

        public async Task<Resort> AddResortAsync(Resort resort)
        {
            _context.Resorts.Add(resort);
            await _context.SaveChangesAsync();
            return resort;
        }

        public async Task<Resort> UpdateResortAsync(long id, Resort resort)
        {
            var existing = await _context.Resorts.FindAsync(id);
            if (existing == null) throw new Exception("Resort not found");

            existing.ResortName = resort.ResortName;
            existing.ResortImageUrl = resort.ResortImageUrl;
            existing.ResortLocation = resort.ResortLocation;
            existing.ResortAvailableStatus = resort.ResortAvailableStatus;
            existing.Price = resort.Price;
            existing.Capacity = resort.Capacity;
            existing.Description = resort.Description;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteResortAsync(long id)
        {
            var resort = await _context.Resorts.FindAsync(id);
            if (resort == null) throw new Exception("Resort not found");

            _context.Resorts.Remove(resort);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}