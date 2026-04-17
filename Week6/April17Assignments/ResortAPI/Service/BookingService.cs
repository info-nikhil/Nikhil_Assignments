using OnlineResortBooking.DTOs.Booking;
using OnlineResortBooking.Models;
using OnlineResortBooking.Repository;

namespace OnlineResortBooking.Service
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IResortRepository _resortRepository;

        public BookingService(IBookingRepository bookingRepository, IResortRepository resortRepository)
        {
            _bookingRepository = bookingRepository;
            _resortRepository = resortRepository;
        }

        public async Task<BookingDto> CreateAsync(BookingDto dto, string userId)
        {
            var resort = await _resortRepository.GetByIdAsync(dto.ResortId);
            if (resort == null) throw new InvalidOperationException("Resort not found");

            var totalDays = (dto.ToDate.Date - dto.FromDate.Date).Days + 1;
            if (totalDays < 1) throw new InvalidOperationException("Invalid dates");

            var totalPrice = resort.Price * dto.NoOfPersons * totalDays;

            var booking = new Booking
            {
                NoOfPersons = dto.NoOfPersons,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                TotalPrice = totalPrice,
                Address = dto.Address,
                ResortId = dto.ResortId,
                UserId = userId,
                Status = "Pending"
            };

            await _bookingRepository.AddAsync(booking);
            await _bookingRepository.SaveChangesAsync();

            dto.BookingId = booking.BookingId;
            dto.TotalPrice = booking.TotalPrice;
            dto.UserId = booking.UserId;
            dto.Status = booking.Status;
            return dto;
        }

        public async Task<bool> DeleteAsync(int id, string callerUserId, bool isAdmin)
        {
            var existing = await _bookingRepository.GetByIdAsync(id);
            if (existing == null) return false;
            if (!isAdmin && existing.UserId != callerUserId) return false;

            _bookingRepository.Remove(existing);
            await _bookingRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BookingDto>> GetAllAsync()
        {
            var items = await _bookingRepository.GetAllWithDetailsAsync();
            return items.Select(MapToDto);
        }

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            var b = await _bookingRepository.GetByIdWithDetailsAsync(id);
            if (b == null) return null;
            return MapToDto(b);
        }

        public async Task<IEnumerable<BookingDto>> GetByUserAsync(string userId)
        {
            var items = await _bookingRepository.GetBookingsByUserAsync(userId);
            return items.Select(MapToDto);
        }

        public async Task<bool> UpdateAsync(int id, BookingDto dto, string callerUserId, bool isAdmin)
        {
            var existing = await _bookingRepository.GetByIdAsync(id);
            if (existing == null) return false;
            if (!isAdmin && existing.UserId != callerUserId) return false;

            if (isAdmin)
            {
                existing.Status = dto.Status ?? existing.Status;
            }
            else
            {
                if (existing.Status != "Pending") return false;
                existing.NoOfPersons = dto.NoOfPersons;
                existing.FromDate = dto.FromDate;
                existing.ToDate = dto.ToDate;
                existing.Address = dto.Address;
            }

            _bookingRepository.Update(existing);
            await _bookingRepository.SaveChangesAsync();
            return true;
        }

        private BookingDto MapToDto(Booking b)
        {
            return new BookingDto
            {
                BookingId = b.BookingId,
                NoOfPersons = b.NoOfPersons,
                FromDate = b.FromDate,
                ToDate = b.ToDate,
                TotalPrice = b.TotalPrice,
                Address = b.Address,
                Status = b.Status,
                ResortId = b.ResortId,
                UserId = b.UserId
            };
        }
    }
}
