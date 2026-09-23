namespace TravellerAI.Domain.ViewModels.Requests;

/// <summary>
/// Trip route: ordered stops in one city. Each stop is a location or an activity.
/// </summary>
public class TripRequest
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    /// <summary>Reorder stops between the first and the last one to minimize the distance.</summary>
    public bool Optimize { get; set; }
    public List<TripStopRequest> Stops { get; set; } = new();
}

public class TripStopRequest
{
    public Guid? LocationId { get; set; }
    public Guid? ActivityId { get; set; }
}
