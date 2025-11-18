using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuardDBcallsAPI.Models;

public partial class Room
{
    
    public int RoomId { get; set; }
    
    public int? UserId { get; set; }
    
    public int? Irid { get; set; }
    
    public string? RoomName { get; set; }

    public int? TotalSmartPlugs { get; set; }

    public int? TotalDevices { get; set; }

    public string? SmartPlugs { get; set; }

    public string? CameraUsername { get; set; }

    public string? CameraPassword { get; set; }

    public string? CameraIpaddress { get; set; }

    public string? AirSensorApikey { get; set; }

    public string? ShellyPmminiDeviceIp { get; set; }

    public string? ShellyFloodDeviceIp { get; set; }

    public virtual ICollection<HazardLog> HazardLogs { get; set; } = new List<HazardLog>();

    public virtual ICollection<Irblaster> Irblasters { get; set; } = new List<Irblaster>();

    public virtual ICollection<SmartPlug> SmartPlugsNavigation { get; set; } = new List<SmartPlug>();

    public virtual User? User { get; set; }
}
