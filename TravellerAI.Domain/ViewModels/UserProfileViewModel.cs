using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class UserProfileViewModel
{
    public Guid UserId { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Age { get; set; }
    public TravelStyle? TravelStyle { get; set; }
    public PersonalityType? PersonalityType { get; set; }
    public BudgetLevel? BudgetLevel { get; set; }
    public CompanionGender CompanionGender { get; set; }
    public string? LookingFor { get; set; }
    /// <summary>ISO 639-1 language codes.</summary>
    public List<string> Languages { get; set; }
    public List<ActivityType> Interests { get; set; }
    public List<ReferenceViewModel> ChosenActivities { get; set; }
    public List<ReferenceViewModel> ChosenTrips { get; set; }
    public List<ReferenceViewModel> PreferredCountries { get; set; }
}
