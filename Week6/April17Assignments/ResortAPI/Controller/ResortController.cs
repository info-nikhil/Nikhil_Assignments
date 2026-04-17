using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.DTOs.Resort;
using OnlineResortBooking.Models;
using OnlineResortBooking.Repository;
using OnlineResortBooking.Service;

namespace OnlineResortBooking.Controller
{
    [ApiController]
    [Route("api/resort")]
    [Authorize]
    public class ResortController : ControllerBase
    {
        private readonly IResortRepository _resortRepository;

        public ResortController(IResortRepository resortRepository)
        {
            _resortRepository = resortRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var resorts = await _resortRepository.GetAllAsync();
            return Ok(resorts);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            var resort = await _resortRepository.GetByIdAsync(id);
            if (resort == null) return NotFound();
            return Ok(resort);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ResortDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var resort = new Resort
            {
                ResortName = dto.ResortName,
                ResortImageUrl = dto.ResortImageUrl,
                ResortLocation = dto.ResortLocation,
                ResortAvailableStatus = dto.ResortAvailableStatus,
                Price = dto.Price,
                Capacity = dto.Capacity,
                Description = dto.Description
            };

            await _resortRepository.AddAsync(resort);
            await _resortRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = resort.ResortId }, resort);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] ResortDto dto)
        {
            var existing = await _resortRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            existing.ResortName = dto.ResortName;
            existing.ResortImageUrl = dto.ResortImageUrl;
            existing.ResortLocation = dto.ResortLocation;
            existing.ResortAvailableStatus = dto.ResortAvailableStatus;
            existing.Price = dto.Price;
            existing.Capacity = dto.Capacity;
            existing.Description = dto.Description;

            _resortRepository.Update(existing);
            await _resortRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _resortRepository.GetByIdAsync(id);
            if (existing == null) return NotFound();

            _resortRepository.Remove(existing);
            await _resortRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}