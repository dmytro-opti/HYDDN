namespace TravellerAI.Domain.Models;

/// <summary>
/// Data to create / update a trip route.
/// </summary>
public class TripDraftModel
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    /// <summary>Reorder stops between the first and the last one to minimize the distance.</summary>
    public bool Optimize { get; set; }
    public List<TripStopDraftModel> Stops { get; set; } = new();
}

/// <summary>
/// Stop is either a location or an activity (at the activity location).
/// </summary>
public class TripStopDraftModel
{
    public Guid? LocationId { get; set; }
    public Guid? ActivityId { get; set; }
}
