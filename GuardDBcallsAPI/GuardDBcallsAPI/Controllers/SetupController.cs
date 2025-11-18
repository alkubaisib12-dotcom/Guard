using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SetupController : ControllerBase
    {
        private readonly ISystemSetupService _setupService;

        public SetupController(ISystemSetupService setupService)
        {
            _setupService = setupService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSetup([FromBody]SetupConfigDto dto)
        {
            var success = await _setupService.SubmitInitialSetupAsync(dto);
            if (!success) return BadRequest();
            return Ok(new { message = "Setup completed successfully." });
        }
    }


}
