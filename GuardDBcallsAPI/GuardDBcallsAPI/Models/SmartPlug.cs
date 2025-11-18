using System;
using System.Collections.Generic;

namespace GuardDBcallsAPI.Models;

public partial class SmartPlug
{
    public int SmartPlugId { get; set; }

    public int? RoomId { get; set; }

    public string? LocationInRoom { get; set; }

    public string? SmartPlugDeviceId { get; set; }

    public string? SmartPlugRegion { get; set; }

    public virtual Room? Room { get; set; }
}
