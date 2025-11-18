using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuardDBcallsAPI.Controllers
{
    public interface IHazardLogService
    {
        Task<HazardLog> CreateHazardLogAsync(HazardLogDto dto);
        Task<HazardLog> GetHazardLogByIdAsync(int id);
        Task<List<HazardLog>> GetHazardLogsByUserIdAsync(int userId);
    }

    public class HazardLogService : IHazardLogService
    {
        private readonly GuardSystemContext _context;

        public HazardLogService(GuardSystemContext context)
        {
            _context = context;
        }

        public async Task<HazardLog> CreateHazardLogAsync(HazardLogDto dto)
        {
            var hazard = new HazardLog
            {
                RoomId = dto.RoomId,
                Time = dto.Time,
                CameraObservations = dto.CameraObservations,
                ActionsTakenByApp = dto.ActionsTakenByApp
            };

            _context.HazardLogs.Add(hazard);
            await _context.SaveChangesAsync();
            return hazard;
        }

        public async Task<HazardLog> GetHazardLogByIdAsync(int id)
        {
            return await _context.HazardLogs.FindAsync(id);
        }

        public async Task<List<HazardLog>> GetHazardLogsByUserIdAsync(int userId)
        {
            return await _context.HazardLogs
                .Where(h => h.Room != null && h.Room.UserId == userId)
                .ToListAsync();
        }
    }



}
