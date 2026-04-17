using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineResortBooking.DTOs.Resort;

namespace OnlineResortBooking.Service
{
    public interface IResortService
    {
        Task<IEnumerable<ResortDto>> GetAllAsync();
        Task<ResortDto?> GetByIdAsync(int id);
        Task<ResortDto> CreateAsync(ResortDto dto);
        Task<bool> UpdateAsync(int id, ResortDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<ResortDto>> GetAvailableAsync();
    }
}
