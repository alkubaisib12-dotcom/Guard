using System;
using System.Collections.Generic;

namespace GuardDBcallsAPI.Models;

public partial class RecommendationsLog
{
    public int RecommendationId { get; set; }

    public int? HazardLogId { get; set; }

    public string? CameraObservations { get; set; }

    public string? Suggestions { get; set; }

    public virtual HazardLog? HazardLog { get; set; }
}
