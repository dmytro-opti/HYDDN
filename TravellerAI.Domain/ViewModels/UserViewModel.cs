namespace TravellerAI.Domain.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    /// <summary>Travel preferences, null until the profile is filled.</summary>
    public UserProfileViewModel? Profile { get; set; }
}
