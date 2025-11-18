using System.ComponentModel.DataAnnotations;

namespace GuardDBcallsAPI.Models
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Range(1, 50)]
        public int TotalRooms { get; set; }
    }
    public class RoomDto
    {
        
        public int UserId { get; set; }

        
        public string RoomName { get; set; }

        public int? TotalSmartPlugs { get; set; }

        public int? TotalDevices { get; set; }

        public string? SmartPlugs { get; set; }
    }


    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SmartPlugDto
    {
        
        public int RoomId { get; set; }

        
        public string LocationInRoom { get; set; }

        
        public string SmartPlugDeviceId { get; set; }

        
        public string SmartPlugRegion { get; set; }
    }
    public class IrblasterDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int TotalDevices { get; set; }

        [Required]
        public string Devices { get; set; } // JSON string or comma-separated
    }

    public class CameraConfigDto
    {
        public int RoomId { get; set; }
        public string CameraUsername { get; set; }

        [Required]
        public string CameraPassword { get; set; }

        [Required]
        public string CameraIpaddress { get; set; }
    }
    public class SensorConfigDto
    {
        public int RoomId { get; set; }
        public string AirSensorApikey { get; set; }

        [Required]
        public string ShellyPmminiDeviceIp { get; set; }

        [Required]
        public string ShellyFloodDeviceIp { get; set; }
    }
    public class HazardLogDto
    {
        
        public int RoomId { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public string CameraObservations { get; set; }

        [Required]
        public string ActionsTakenByApp { get; set; }
    }

    public class RecommendationsLogDto
    {
        
        public int HazardLogId { get; set; }

        [Required]
        public string CameraObservations { get; set; }

        [Required]
        public string Suggestions { get; set; }
    }
    public class SetupConfigDto
    {
        
        public int UserId { get; set; }

        [Required]
        public List<RoomDto> Rooms { get; set; }

        
        public List<SmartPlugDto> SmartPlugs { get; set; }

        [Required]
        public List<IrblasterDto> Irblasters { get; set; }

        [Required]
        public List<CameraConfigDto> Cameras { get; set; }

        [Required]
        public List<SensorConfigDto> Sensors { get; set; }
    }
    public class DashboardDto
    {
        public List<Room> Rooms { get; set; }
        public List<SmartPlug> SmartPlugs { get; set; }
        public List<Irblaster> Irblasters { get; set; }
        public List<HazardLog> Hazards { get; set; }
    }








}
