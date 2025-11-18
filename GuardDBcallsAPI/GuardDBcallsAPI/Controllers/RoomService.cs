using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuardDBcallsAPI.Controllers
{
    public interface IRoomService
    {
        Task<Room> CreateRoomAsync([FromBody]RoomDto dto);
        Task<Room> GetRoomByIdAsync(int id);
        Task<Room> UpdateRoomAsync(int id, RoomDto dto);
        Task<bool> DeleteRoomAsync(int id);

        Task<List<Room>> GetRoomsByUserIdAsync(int userId);

        Task<bool> UpdateCameraConfigAsync(int roomId, [FromBody] CameraConfigDto dto);
        Task<bool> UpdateSensorConfigAsync(int roomId, SensorConfigDto dto);


    }

    public class RoomService : IRoomService
    {
        private readonly GuardSystemContext _context;

        public RoomService(GuardSystemContext context)
        {
            _context = context;
        }

        public async Task<Room> CreateRoomAsync([FromBody]RoomDto dto)
        {
            var room = new Room
            {
                UserId = dto.UserId,
                RoomName = dto.RoomName,
                TotalSmartPlugs = dto.TotalSmartPlugs,
                TotalDevices = dto.TotalDevices,
                SmartPlugs = dto.SmartPlugs
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<Room> GetRoomByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task<Room> UpdateRoomAsync(int id, [FromBody] RoomDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return null;

            room.RoomName = dto.RoomName;
            room.TotalSmartPlugs = dto.TotalSmartPlugs;
            room.TotalDevices = dto.TotalDevices;
            room.SmartPlugs = dto.SmartPlugs;

            await _context.SaveChangesAsync();
            return room;
        }

        public async Task<bool> DeleteRoomAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return false;

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Room>> GetRoomsByUserIdAsync(int userId)
        {
            return await _context.Rooms
                .Where(r => r.UserId == userId)
                .ToListAsync();

        }

        public async Task<bool> UpdateCameraConfigAsync(int roomId, [FromBody] CameraConfigDto dto)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.CameraUsername = dto.CameraUsername;
            room.CameraPassword = dto.CameraPassword;
            room.CameraIpaddress = dto.CameraIpaddress;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateSensorConfigAsync(int roomId, SensorConfigDto dto)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.AirSensorApikey = dto.AirSensorApikey;
            room.ShellyPmminiDeviceIp = dto.ShellyPmminiDeviceIp;
            room.ShellyFloodDeviceIp = dto.ShellyFloodDeviceIp;

            await _context.SaveChangesAsync();
            return true;
        }


    }




























}
