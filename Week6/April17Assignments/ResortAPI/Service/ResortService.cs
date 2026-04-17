using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineResortBooking.DTOs.Resort;
using OnlineResortBooking.Models;
using OnlineResortBooking.Repository;

namespace OnlineResortBooking.Service
{
    public class ResortService : IResortService
    {
        private readonly IResortRepository _resortRepository;

        public ResortService(IResortRepository resortRepository)
        {
            _resortRepository = resortRepository;
        }

        public async Task<ResortDto> CreateAsync(ResortDto dto)
        {
            var entity = new Resort
            {
                ResortName = dto.ResortName,
                ResortImageUrl = dto.ResortImageUrl,
                ResortLocation = dto.ResortLocation,
                ResortAvailableStatus = dto.ResortAvailableStatus,
                Price = dto.Price,
                Capacity = dto.Capacity,
                Description = dto.Description
            };

            await _resortRepository.AddAsync(entity);
            await _resortRepository.SaveChangesAsync();

            dto.ResortId = entity.ResortId;
            return dto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _resortRepository.GetByIdAsync(id);
            if (existing == null) return false;

            _resortRepository.Remove(existing);
            await _resortRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResortDto>> GetAllAsync()
        {
            var items = await _resortRepository.GetAllAsync();
            return items.Select(MapToDto);
        }

        public async Task<ResortDto?> GetByIdAsync(int id)
        {
            var r = await _resortRepository.GetByIdAsync(id);
            return r == null ? null : MapToDto(r);
        }

        public async Task<bool> UpdateAsync(int id, ResortDto dto)
        {
            var existing = await _resortRepository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.ResortName = dto.ResortName;
            existing.ResortImageUrl = dto.ResortImageUrl;
            existing.ResortLocation = dto.ResortLocation;
            existing.ResortAvailableStatus = dto.ResortAvailableStatus;
            existing.Price = dto.Price;
            existing.Capacity = dto.Capacity;
            existing.Description = dto.Description;

            _resortRepository.Update(existing);
            await _resortRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResortDto>> GetAvailableAsync()
        {
            var items = await _resortRepository.GetAvailableResortsAsync();
            return items.Select(MapToDto);
        }

        private static ResortDto MapToDto(Resort r)
        {
            return new ResortDto
            {
                ResortId = r.ResortId,
                ResortName = r.ResortName,
                ResortImageUrl = r.ResortImageUrl,
                ResortLocation = r.ResortLocation,
                ResortAvailableStatus = r.ResortAvailableStatus,
                Price = r.Price,
                Capacity = r.Capacity,
                Description = r.Description
            };
        }
    }
}
