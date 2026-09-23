namespace TravellerAI.Domain.Entities;

/// <summary>
/// Stop of a trip: a location or an activity (at the activity location).
/// </summary>
public class TripStopEntity : BaseEntity
{
    public Guid TripId { get; set; }
    public virtual TripEntity Trip { get; set; } = null!;

    /// <summary>Position in the route, starting from 0.</summary>
    public int Order { get; set; }

    public Guid LocationId { get; set; }
    public virtual LocationEntity Location { get; set; } = null!;

    public Guid? ActivityId { get; set; }
    public virtual ActivityEntity? Activity { get; set; }

    /// <summary>Great-circle distance from the previous stop, km.</summary>
    public double DistanceFromPreviousKm { get; set; }
}
