using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;
        private readonly IRoomService _roomService;

        public UsersController(IUserService userService, JwtService jwtService, IRoomService roomService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _roomService = roomService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var user = await _userService.RegisterAsync(dto);
                return Ok(new { user.UserId, user.Username, user.TotalRooms });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Register] ERROR: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(dto);
                var token = _jwtService.GenerateToken(user.Username, "User");
                return Ok(new { user.UserId, user.Username});
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Login] ERROR: {ex.Message}");
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(new { user.UserId, user.Username, user.TotalRooms });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetUser] ERROR: {ex.Message}");
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("{id}/rooms")]
        public async Task<IActionResult> GetUserRooms(int id)
        {
            try
            {
                var rooms = await _roomService.GetRoomsByUserIdAsync(id);
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetUserRooms] ERROR: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("ping")]
        public IActionResult Ping() => Ok("pong");

        [AllowAnonymous]
        [HttpPost("verify-room-count")]
        public async Task<IActionResult> VerifyRoomCount([FromBody] dynamic payload)
        {
            string username = payload?.username;
            int claimedRoomCount = payload?.roomCount;

            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is required.");

            bool isMatch = await _userService.VerifyRoomCountAsync(username, claimedRoomCount);
            return isMatch ? Ok("Verification successful.") : Unauthorized("Room count does not match.");
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] dynamic payload)
        {
            string username = payload?.username;
            string newPassword = payload?.newPassword;
            string confirmPassword = payload?.confirmPassword;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
                return BadRequest("All fields are required.");

            if (newPassword != confirmPassword)
                return BadRequest("Passwords do not match.");

            bool success = await _userService.ResetPasswordAsync(username, newPassword);
            return success ? Ok("Password reset successful.") : BadRequest("Failed to reset password.");
        }






    }
}
