using System;
using System.Collections.Generic;

namespace GuardDBcallsAPI.Models;

public partial class HazardLog
{
    public int HazardLogId { get; set; }

    public int? RoomId { get; set; }

    public DateTime? Time { get; set; }

    public string? CameraObservations { get; set; }

    public string? ActionsTakenByApp { get; set; }

    public virtual ICollection<RecommendationsLog> RecommendationsLogs { get; set; } = new List<RecommendationsLog>();

    public virtual Room? Room { get; set; }
}
