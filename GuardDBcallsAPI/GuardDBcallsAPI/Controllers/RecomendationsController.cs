using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationsService _recommendationsService;

        public RecommendationsController(IRecommendationsService recommendationsService)
        {
            _recommendationsService = recommendationsService;
        }

        [HttpPost]
        public async Task<IActionResult> LogRecommendation([FromBody]RecommendationsLogDto dto)
        {
            var rec = await _recommendationsService.CreateRecommendationAsync(dto);
            return Ok(rec);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecommendation(int id)
        {
            var rec = await _recommendationsService.GetRecommendationByIdAsync(id);
            if (rec == null) return NotFound();
            return Ok(rec);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRecommendationsForUser(int userId)
        {
            var recs = await _recommendationsService.GetRecommendationsByUserIdAsync(userId);
            return Ok(recs);
        }
    }

}
