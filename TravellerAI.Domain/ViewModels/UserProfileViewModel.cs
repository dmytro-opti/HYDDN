namespace TravellerAI.Domain.ViewModels;

public class UserProfileViewModel
{
    public Guid UserId { get; set; }
    public List<string> Interests { get; set; }
    public string TravelStyle { get; set; }
    public List<string> Points { get; set; }
    public string LookingFor { get; set; }
    public List<string> Languages { get; set; }
    public List<string> PersonalityType { get; set; }
    public int Age { get; set; }
    public List<string> Genders { get; set; }
    public string Destination { get; set; }
    public DateTime? JourneyDate { get; set; }
    public List<string> ChoosenActivity { get; set; }
    public List<string> ChoosenTrip { get; set; }
    public List<string> MoneyAmount { get; set; }
}
