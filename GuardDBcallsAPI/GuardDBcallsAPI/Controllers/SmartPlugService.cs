using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GuardDBcallsAPI.Controllers
{
    public interface ISmartPlugService
    {
        Task<SmartPlug> AddSmartPlugAsync([FromBody]SmartPlugDto dto);
        Task<SmartPlug> GetSmartPlugByIdAsync(int id);
        Task<SmartPlug> UpdateSmartPlugAsync(int id, [FromBody]SmartPlugDto dto);
        Task<bool> DeleteSmartPlugAsync(int id);
    }

    public class SmartPlugService : ISmartPlugService
    {
        private readonly GuardSystemContext _context;

        public SmartPlugService(GuardSystemContext context)
        {
            _context = context;
        }

        public async Task<SmartPlug> AddSmartPlugAsync([FromBody]SmartPlugDto dto)
        {
            var plug = new SmartPlug
            {
                RoomId = dto.RoomId,
                LocationInRoom = dto.LocationInRoom,
                SmartPlugDeviceId = dto.SmartPlugDeviceId,
                SmartPlugRegion = dto.SmartPlugRegion
            };

            _context.SmartPlugs.Add(plug);
            await _context.SaveChangesAsync();
            return plug;
        }

        public async Task<SmartPlug> GetSmartPlugByIdAsync(int id)
        {
            return await _context.SmartPlugs.FindAsync(id);
        }

        public async Task<SmartPlug> UpdateSmartPlugAsync(int id,SmartPlugDto dto)
        {
            var plug = await _context.SmartPlugs.FindAsync(id);
            if (plug == null) return null;

            plug.RoomId = dto.RoomId;
            plug.LocationInRoom = dto.LocationInRoom;
            plug.SmartPlugDeviceId = dto.SmartPlugDeviceId;
            plug.SmartPlugRegion = dto.SmartPlugRegion;

            await _context.SaveChangesAsync();
            return plug;
        }

        public async Task<bool> DeleteSmartPlugAsync(int id)
        {
            var plug = await _context.SmartPlugs.FindAsync(id);
            if (plug == null) return false;

            _context.SmartPlugs.Remove(plug);
            await _context.SaveChangesAsync();
            return true;
        }
    }




}
