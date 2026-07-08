using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

public class OptimalWayModel
{
    public Guid OptimalWayId { get; set; }
    public Guid TripId { get; set; }
    public LocationModel StartPoint { get; set; }
    public LocationModel EndPoint { get; set; }
    public RouteOptimizationType OptimizationType { get; set; }
    public TravelMode TravelMode { get; set; }
    public List<LocationModel> Waypoints { get; set; } = new();
}