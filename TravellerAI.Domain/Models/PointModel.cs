namespace TravellerAI.Domain.Models;

/// <summary>
/// Point of a route on the map.
/// </summary>
public class PointModel
{
    public Guid LocationId { get; set; }
    public string? Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    /// <summary>Position in the route, starting from 0.</summary>
    public int Order { get; set; }
    /// <summary>Great-circle distance from the previous point, km.</summary>
    public double DistanceFromPreviousKm { get; set; }
}
