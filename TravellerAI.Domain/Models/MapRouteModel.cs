namespace TravellerAI.Domain.Models;

public class MapRouteModel
{
    public LocationModel StartPoint { get; set; }
    public LocationModel EndPoint { get; set; }
    public List<LocationModel> WayPoints { get; set; }
    public double TotalDistance { get; set; }
}