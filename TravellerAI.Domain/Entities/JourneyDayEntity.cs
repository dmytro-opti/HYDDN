namespace TravellerAI.Domain.Entities;

/// <summary>
/// One day of a journey with the scheduled trip (null - free day).
/// </summary>
public class JourneyDayEntity : BaseEntity
{
    public Guid JourneyId { get; set; }
    public virtual JourneyEntity Journey { get; set; } = null!;

    /// <summary>Date without time.</summary>
    public DateTime Date { get; set; }

    public Guid? TripId { get; set; }
    public virtual TripEntity? Trip { get; set; }
}
