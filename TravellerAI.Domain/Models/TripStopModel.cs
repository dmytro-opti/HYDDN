namespace TravellerAI.Domain.Models;

public class TripStopModel
{
    public int Order { get; set; }
    public Guid LocationId { get; set; }
    public LocationModel Location { get; set; }
    public Guid? ActivityId { get; set; }
    public string? ActivityName { get; set; }
    public decimal? ActivityPrice { get; set; }
    public double DistanceFromPreviousKm { get; set; }
}
