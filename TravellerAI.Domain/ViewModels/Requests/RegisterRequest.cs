namespace TravellerAI.Domain.ViewModels.Requests;

public class RegisterRequest
{
    public string Email { get; set; }
    /// <summary>At least 8 characters with an uppercase letter, a lowercase letter and a digit.</summary>
    public string Password { get; set; }
    /// <summary>Display name.</summary>
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
