namespace TravellerAI.Domain.ViewModels;

public class AuthUserViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public List<string> Roles { get; set; }
}
