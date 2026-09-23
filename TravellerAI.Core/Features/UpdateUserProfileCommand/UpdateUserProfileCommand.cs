using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.UpdateUserProfileCommand;

/// <summary>
/// Updates user names and travel preferences. Email and password have their own commands.
/// </summary>
public class UpdateUserProfileCommand : IRequest<UserModel>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public TravelStyle? TravelStyle { get; set; }
    public PersonalityType? PersonalityType { get; set; }
    public BudgetLevel? BudgetLevel { get; set; }
    public CompanionGender CompanionGender { get; set; }
    public string? LookingFor { get; set; }
    public List<string> Languages { get; set; } = new();
    public List<ActivityType> Interests { get; set; } = new();
    public List<Guid> ChosenActivityIds { get; set; } = new();
    public List<Guid> ChosenTripIds { get; set; } = new();
    public List<Guid> PreferredCountryIds { get; set; } = new();
}
