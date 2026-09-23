using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels.Requests;

public class UpdateUserProfileRequest
{
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public TravelStyle? TravelStyle { get; set; }
    public PersonalityType? PersonalityType { get; set; }
    public BudgetLevel? BudgetLevel { get; set; }
    public CompanionGender CompanionGender { get; set; }
    public string? LookingFor { get; set; }
    /// <summary>ISO 639-1 language codes, e.g. ["uk", "en"].</summary>
    public List<string> Languages { get; set; } = new();
    public List<ActivityType> Interests { get; set; } = new();
    public List<Guid> ChosenActivityIds { get; set; } = new();
    public List<Guid> ChosenTripIds { get; set; } = new();
    public List<Guid> PreferredCountryIds { get; set; } = new();
}
