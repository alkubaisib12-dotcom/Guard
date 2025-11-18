using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmartPlugsController : ControllerBase
    {
        private readonly ISmartPlugService _smartPlugService;

        public SmartPlugsController(ISmartPlugService smartPlugService)
        {
            _smartPlugService = smartPlugService;
        }

        [HttpPost]
        public async Task<IActionResult> AddSmartPlug([FromBody]SmartPlugDto dto)
        {
            var plug = await _smartPlugService.AddSmartPlugAsync(dto);
            return Ok(plug);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSmartPlug(int id)
        {
            var plug = await _smartPlugService.GetSmartPlugByIdAsync(id);
            if (plug == null) return NotFound();
            return Ok(plug);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSmartPlug(int id, [FromBody]SmartPlugDto dto)
        {
            var plug = await _smartPlugService.UpdateSmartPlugAsync(id, dto);
            if (plug == null) return NotFound();
            return Ok(plug);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSmartPlug(int id)
        {
            var success = await _smartPlugService.DeleteSmartPlugAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }

}
