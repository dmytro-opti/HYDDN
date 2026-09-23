using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Entities;

/// <summary>
/// Travel preferences of the user, used for recommendations and companion matching.
/// </summary>
public class UserInfoEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public DateTime? BirthDate { get; set; }
    public TravelStyle? TravelStyle { get; set; }
    public PersonalityType? PersonalityType { get; set; }
    public BudgetLevel? BudgetLevel { get; set; }
    public CompanionGender CompanionGender { get; set; }
    /// <summary>Free text: what the user is looking for in a journey.</summary>
    public string? LookingFor { get; set; }
    /// <summary>ISO 639-1 language codes, e.g. uk, en.</summary>
    public List<string> Languages { get; set; } = new();
    public List<ActivityType> Interests { get; set; } = new();

    public virtual ICollection<ActivityEntity> ChosenActivities { get; set; } = new List<ActivityEntity>();
    public virtual ICollection<TripEntity> ChosenTrips { get; set; } = new List<TripEntity>();
    public virtual ICollection<CountryEntity> PreferredCountries { get; set; } = new List<CountryEntity>();
}
