using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody]RoomDto dto)
        {
            var room = await _roomService.CreateRoomAsync(dto);
            return Ok(new { room.RoomId, room.RoomName });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoom(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomDto dto)
        {
            var room = await _roomService.UpdateRoomAsync(id, dto);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var success = await _roomService.DeleteRoomAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}/camera")]
        public async Task<IActionResult> UpdateCameraConfig(int id, [FromBody] CameraConfigDto dto)
        {
            var success = await _roomService.UpdateCameraConfigAsync(id, dto);
            if (!success) return NotFound();
            return Ok(new { message = "Camera configuration updated." });
        }

        [HttpPut("{id}/sensor")]
        public async Task<IActionResult> UpdateSensorConfig(int id, [FromBody] SensorConfigDto dto)
        {
            var success = await _roomService.UpdateSensorConfigAsync(id, dto);
            if (!success) return NotFound();
            return Ok(new { message = "Sensor configuration updated." });
        }

    }


}
