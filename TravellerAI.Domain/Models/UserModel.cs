namespace TravellerAI.Domain.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    /// <summary>Travel preferences, null until the user fills the profile.</summary>
    public UserInfoModel? Profile { get; set; }
    public IEnumerable<JourneyModel> Journeys { get; set; }
}
