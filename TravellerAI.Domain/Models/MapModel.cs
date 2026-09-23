namespace TravellerAI.Domain.Models;

/// <summary>
/// Route between locations (e.g. a one day trip).
/// </summary>
public class MapModel
{
    public List<PointModel> Points { get; set; } = new();
    public double TotalDistanceKm { get; set; }
    public bool IsOptimized { get; set; }
}
