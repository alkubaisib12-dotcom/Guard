using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDashboard(int userId)
        {
            var dashboard = await _dashboardService.GetDashboardAsync(userId);
            return Ok(dashboard);
        }
    }

}
