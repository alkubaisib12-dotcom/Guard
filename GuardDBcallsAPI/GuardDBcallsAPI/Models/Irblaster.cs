using System;
using System.Collections.Generic;

namespace GuardDBcallsAPI.Models;

public partial class Irblaster
{
    public int Irid { get; set; }

    public int? UserId { get; set; }

    public int? RoomId { get; set; }

    public int? TotalDevices { get; set; }

    public string? Devices { get; set; }

    public virtual Room? Room { get; set; }

    public virtual User? User { get; set; }
}
