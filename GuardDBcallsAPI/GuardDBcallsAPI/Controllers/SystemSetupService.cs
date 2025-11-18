using GuardDBcallsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GuardDBcallsAPI.Controllers
{
    public interface ISystemSetupService
    {
        Task<bool> SubmitInitialSetupAsync(SetupConfigDto dto);
    }




    public class SystemSetupService : ISystemSetupService
    {
        private readonly IRoomService _roomService;
        private readonly ISmartPlugService _smartPlugService;
        private readonly IIrblasterService _irblasterService;




        public SystemSetupService(IRoomService roomService,
                                  ISmartPlugService smartPlugService,
                                  IIrblasterService irblasterService)
        {
            _roomService = roomService;
            _smartPlugService = smartPlugService;
            _irblasterService = irblasterService;

        }

        public async Task<bool> SubmitInitialSetupAsync(SetupConfigDto dto)
        {
            foreach (var room in dto.Rooms)
                await _roomService.CreateRoomAsync(room);

            foreach (var plug in dto.SmartPlugs)
                await _smartPlugService.AddSmartPlugAsync(plug);

            foreach (var ir in dto.Irblasters)
                await _irblasterService.AddIrblasterAsync(ir);

            foreach (var camera in dto.Cameras)
                await _roomService.UpdateCameraConfigAsync(camera.RoomId, camera);

            foreach (var sensor in dto.Sensors)
                await _roomService.UpdateSensorConfigAsync(sensor.RoomId, sensor);





            return true;
        }




    }


}
