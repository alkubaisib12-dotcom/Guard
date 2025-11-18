using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuardDBcallsAPI.Controllers
{

    public interface IRecommendationsService
    {
        Task<RecommendationsLog> CreateRecommendationAsync(RecommendationsLogDto dto);
        Task<RecommendationsLog> GetRecommendationByIdAsync(int id);
        Task<List<RecommendationsLog>> GetRecommendationsByUserIdAsync(int userId);
    }

    public class RecommendationsService : IRecommendationsService
    {
        private readonly GuardSystemContext _context;

        public RecommendationsService(GuardSystemContext context)
        {
            _context = context;
        }

        public async Task<RecommendationsLog> CreateRecommendationAsync(RecommendationsLogDto dto)
        {
            var rec = new RecommendationsLog
            {
                HazardLogId = dto.HazardLogId,
                CameraObservations = dto.CameraObservations,
                Suggestions = dto.Suggestions
            };

            _context.RecommendationsLogs.Add(rec);
            await _context.SaveChangesAsync();
            return rec;
        }

        public async Task<RecommendationsLog> GetRecommendationByIdAsync(int id)
        {
            return await _context.RecommendationsLogs.FindAsync(id);
        }

        public async Task<List<RecommendationsLog>> GetRecommendationsByUserIdAsync(int userId)
        {
            return await _context.RecommendationsLogs
                .Where(r => r.HazardLog != null &&
                            r.HazardLog.Room != null &&
                            r.HazardLog.Room.UserId == userId)
                .ToListAsync();
        }
    }














}
