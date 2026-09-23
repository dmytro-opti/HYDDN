namespace TravellerAI.Domain.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public List<string> Interests { get; set; }
    public string TravelStyle { get; set; }
    public string LookingFor { get; set; }
    public List<string> Languages { get; set; }
    public List<string> PersonalityType { get; set; }
    public int Age { get; set; }
    public List<string> ChoosenActivity { get; set; }
    public List<string> ChoosenTrip { get; set; }
    public List<string> MoneyAmount { get; set; }
}
