using GuardDBcallsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GuardDBcallsAPI.Controllers
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardAsync(int userId);
    }

    public class DashboardService : IDashboardService
    {
        private readonly GuardSystemContext _context;

        public DashboardService(GuardSystemContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardAsync(int userId)
        {
            var rooms = await _context.Rooms
                .Where(r => r.UserId == userId)
                .ToListAsync();

            var roomIds = rooms.Select(r => r.RoomId).ToList();

            var plugs = await _context.SmartPlugs
                .Where(p => p.RoomId.HasValue && roomIds.Contains(p.RoomId.Value))
                .ToListAsync();

            var irs = await _context.Irblasters
                .Where(i => i.UserId == userId)
                .ToListAsync();

            var hazards = await _context.HazardLogs
                .Where(h => h.RoomId.HasValue && roomIds.Contains(h.RoomId.Value))
                .ToListAsync();

            return new DashboardDto
            {
                Rooms = rooms,
                SmartPlugs = plugs,
                Irblasters = irs,
                Hazards = hazards
            };
        }



    }


}
