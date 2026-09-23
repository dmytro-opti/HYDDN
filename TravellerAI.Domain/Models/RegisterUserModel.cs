namespace TravellerAI.Domain.Models;

public class RegisterUserModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    /// <summary>Display name.</summary>
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
