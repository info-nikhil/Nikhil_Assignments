using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineResortBooking.API.Models;
using OnlineResortBooking.API.Services.Interfaces;

namespace OnlineResortBooking.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/resort")]
    public class ResortController : ControllerBase
    {
        private readonly IResortService _service;

        public ResortController(IResortService service)
        {
            _service = service;
        }

        // GET: api/resort
        [HttpGet]
        public async Task<IActionResult> GetAllResorts()
        {
            var resorts = await _service.GetAllResortsAsync();
            return Ok(resorts);
        }

        // GET: api/resort/{resortId}
        [HttpGet("{resortId}")]
        public async Task<IActionResult> GetResortById(long resortId)
        {
            var resort = await _service.GetResortByIdAsync(resortId);
            return Ok(resort);
        }

        // POST: api/resort
        [HttpPost]
        public async Task<IActionResult> AddResort([FromBody] Resort resort)
        {
            if (resort == null)
                return BadRequest("Invalid resort data");

            if (string.IsNullOrWhiteSpace(resort.ResortName))
                return BadRequest("Resort name is required");

            var created = await _service.AddResortAsync(resort);

            return StatusCode(201, created);
        }

        // PUT: api/resort/{resortId}
        [HttpPut("{resortId}")]
        public async Task<IActionResult> UpdateResort(long resortId, [FromBody] Resort resort)
        {
            if (resort == null)
                return BadRequest("Invalid resort data");

            var updated = await _service.UpdateResortAsync(resortId, resort);

            return Ok(updated);
        }

        // DELETE: api/resort/{resortId}
        [HttpDelete("{resortId}")]
        public async Task<IActionResult> DeleteResort(long resortId)
        {
            var result = await _service.DeleteResortAsync(resortId);

            return Ok(new
            {
                message = "Resort deleted successfully",
                result = result
            });
        }
    }
}