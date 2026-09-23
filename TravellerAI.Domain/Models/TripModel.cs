namespace TravellerAI.Domain.Models;

/// <summary>
/// One-day route between locations and activities in one city.
/// </summary>
public class TripModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public bool IsPublic { get; set; }
    public Guid CountryId { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public double DistanceKm { get; set; }
    public Guid AuthorId { get; set; }
    public List<TripStopModel> Stops { get; set; } = new();
}
