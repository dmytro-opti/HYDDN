namespace TravellerAI.Domain.Models;

public class MapModel
{
    Guid MapId { get; set; }
    LocationModel Origin { get; set; }
    LocationModel Destination { get; set; }
    public List<LocationModel> Waypoints { get; set; } = new();
    public double DistanceInKilometers { get; set; }
    
}