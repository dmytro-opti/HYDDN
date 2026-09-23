using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

/// <summary>
/// Travel preferences of the user (recommendations and companion matching).
/// </summary>
public class UserInfoModel
{
    public Guid UserId { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? Age => BirthDate.HasValue ? CalculateAge(BirthDate.Value, DateTime.UtcNow) : null;
    public TravelStyle? TravelStyle { get; set; }
    public PersonalityType? PersonalityType { get; set; }
    public BudgetLevel? BudgetLevel { get; set; }
    public CompanionGender CompanionGender { get; set; }
    public string? LookingFor { get; set; }
    /// <summary>ISO 639-1 language codes.</summary>
    public List<string> Languages { get; set; } = new();
    public List<ActivityType> Interests { get; set; } = new();
    public List<ReferenceModel> ChosenActivities { get; set; } = new();
    public List<ReferenceModel> ChosenTrips { get; set; } = new();
    public List<ReferenceModel> PreferredCountries { get; set; } = new();

    public static int CalculateAge(DateTime birthDate, DateTime today)
    {
        var age = today.Year - birthDate.Year;
        return birthDate.Date > today.Date.AddYears(-age) ? age - 1 : age;
    }
}
