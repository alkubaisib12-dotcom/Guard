using GuardDBcallsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GuardDBcallsAPI.Controllers
{

    public interface IUserService
    {
        Task<User> RegisterAsync(RegisterDto dto);
        Task<User> AuthenticateAsync(LoginDto dto);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);

        Task<bool> VerifyRoomCountAsync(string username, int claimedRoomCount);
        Task<bool> ResetPasswordAsync(string username, string newPassword);
    }







    public class UserService : IUserService
    {
        private readonly GuardSystemContext _context;

        public UserService(GuardSystemContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterAsync(RegisterDto dto)
        {
            try
            {
                Console.WriteLine($"[RegisterAsync] Incoming: Username={dto.Username}, TotalRooms={dto.TotalRooms}");

                if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                    throw new ArgumentException("Username and Password are required.");

                if (dto.TotalRooms < 1 || dto.TotalRooms > 50)
                    throw new ArgumentOutOfRangeException("TotalRooms must be between 1 and 50.");

                var existing = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
                if (existing != null)
                    throw new InvalidOperationException("Username already exists.");

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                var user = new User
                {
                    Username = dto.Username,
                    PasswordHash = passwordHash,
                    TotalRooms = dto.TotalRooms
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                Console.WriteLine("[RegisterAsync] User created successfully.");
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RegisterAsync] ERROR: {ex.Message}");
                throw;
            }
        }

        public async Task<User> AuthenticateAsync(LoginDto dto)
        {
            try
            {
                Console.WriteLine($"[AuthenticateAsync] Incoming: Username={dto.Username}");

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                    throw new UnauthorizedAccessException("Invalid credentials.");

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthenticateAsync] ERROR: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    throw new KeyNotFoundException("User not found.");

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetUserByIdAsync] ERROR: {ex.Message}");
                throw;
            }
        }


        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) throw new KeyNotFoundException("User not found.");
            return user;
        }

        public async Task<bool> VerifyRoomCountAsync(string username, int claimedRoomCount)
        {
            var user = await GetUserByUsernameAsync(username);
            return user.TotalRooms == claimedRoomCount;
        }

        public async Task<bool> ResetPasswordAsync(string username, string newPassword)
        {
            var user = await GetUserByUsernameAsync(username);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }





    }
}
