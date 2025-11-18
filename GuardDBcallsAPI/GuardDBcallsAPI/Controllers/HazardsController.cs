using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HazardsController : ControllerBase
    {
        private readonly IHazardLogService _hazardService;

        public HazardsController(IHazardLogService hazardService)
        {
            _hazardService = hazardService;
        }

        [HttpPost]
        public async Task<IActionResult> LogHazard([FromBody]HazardLogDto dto)
        {
            var hazard = await _hazardService.CreateHazardLogAsync(dto);
            return Ok(hazard);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHazard(int id)
        {
            var hazard = await _hazardService.GetHazardLogByIdAsync(id);
            if (hazard == null) return NotFound();
            return Ok(hazard);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetHazardsForUser(int userId)
        {
            var hazards = await _hazardService.GetHazardLogsByUserIdAsync(userId);
            return Ok(hazards);
        }
    }

}
