namespace TravellerAI.Domain.Entities;

/// <summary>
/// Reusable one-day route: ordered stops (locations and activities) in one city.
/// Journeys schedule trips by day; a public trip can be used by any user.
/// </summary>
public class TripEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double Rating { get; set; }
    public bool IsPublic { get; set; }

    public Guid CountryId { get; set; }
    public virtual CountryEntity Country { get; set; } = null!;
    public string City { get; set; } = string.Empty;

    /// <summary>Total route distance, km.</summary>
    public double DistanceKm { get; set; }

    /// <summary>Author of the trip.</summary>
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public virtual ICollection<TripStopEntity> Stops { get; set; } = new List<TripStopEntity>();
    public virtual ICollection<JourneyDayEntity> Days { get; set; } = new List<JourneyDayEntity>();

    /// <summary>Profiles which have chosen this trip.</summary>
    public virtual ICollection<UserInfoEntity> ChosenBy { get; set; } = new List<UserInfoEntity>();
}
