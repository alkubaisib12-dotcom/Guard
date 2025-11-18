using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    public interface IIrblasterService
    {
        Task<Irblaster> AddIrblasterAsync(IrblasterDto dto);
        Task<Irblaster> GetIrblasterByIdAsync(int id);
        Task<List<string>> GetDevicesByIrblasterIdAsync(int id);
        Task<bool> AddDeviceToIrblasterAsync(int id, string deviceName);
    }

    public class IrblasterService : IIrblasterService
    {
        private readonly GuardSystemContext _context;

        public IrblasterService(GuardSystemContext context)
        {
            _context = context;
        }

        public async Task<Irblaster> AddIrblasterAsync(IrblasterDto dto)
        {
            var ir = new Irblaster
            {
                UserId = dto.UserId,
                RoomId = dto.RoomId,
                TotalDevices = dto.TotalDevices,
                Devices = dto.Devices
            };

            _context.Irblasters.Add(ir);
            await _context.SaveChangesAsync();
            return ir;
        }

        public async Task<Irblaster> GetIrblasterByIdAsync(int id)
        {
            return await _context.Irblasters.FindAsync(id);
        }

        public async Task<List<string>> GetDevicesByIrblasterIdAsync(int id)
        {
            var ir = await _context.Irblasters.FindAsync(id);
            if (ir == null || string.IsNullOrEmpty(ir.Devices)) return new List<string>();

            return ir.Devices.Split(',').Select(d => d.Trim()).ToList();
        }

        public async Task<bool> AddDeviceToIrblasterAsync(int id, string deviceName)
        {
            var ir = await _context.Irblasters.FindAsync(id);
            if (ir == null) return false;

            var devices = string.IsNullOrEmpty(ir.Devices)
                ? new List<string>()
                : ir.Devices.Split(',').Select(d => d.Trim()).ToList();

            devices.Add(deviceName);
            ir.Devices = string.Join(",", devices);
            ir.TotalDevices = devices.Count;

            await _context.SaveChangesAsync();
            return true;
        }
    }


}
