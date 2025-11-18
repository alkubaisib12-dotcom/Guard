using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GuardDBcallsAPI.Models;

public partial class User
{
    
    public int UserId { get; set; }
    
    public string Username { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;
    
    public int? TotalRooms { get; set; }

    public virtual ICollection<Irblaster> Irblasters { get; set; } = new List<Irblaster>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
