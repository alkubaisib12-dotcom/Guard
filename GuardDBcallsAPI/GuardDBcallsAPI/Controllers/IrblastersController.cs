using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IrblastersController : ControllerBase
    {
        private readonly IIrblasterService _irblasterService;

        public IrblastersController(IIrblasterService irblasterService)
        {
            _irblasterService = irblasterService;
        }

        [HttpPost]
        public async Task<IActionResult> AddIrblaster([FromBody]IrblasterDto dto)
        {
            var ir = await _irblasterService.AddIrblasterAsync(dto);
            return Ok(ir);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIrblaster(int id)
        {
            var ir = await _irblasterService.GetIrblasterByIdAsync(id);
            if (ir == null) return NotFound();
            return Ok(ir);
        }

        [HttpGet("{id}/devices")]
        public async Task<IActionResult> GetDevices(int id)
        {
            var devices = await _irblasterService.GetDevicesByIrblasterIdAsync(id);
            return Ok(devices);
        }

        [HttpPost("{id}/devices")]
        public async Task<IActionResult> AddDevice(int id, [FromBody] string deviceName)
        {
            var success = await _irblasterService.AddDeviceToIrblasterAsync(id, deviceName);
            if (!success) return NotFound();
            return Ok(new { message = "Device added successfully." });
        }
    }






}
