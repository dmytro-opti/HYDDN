namespace TravellerAI.Domain.Entities;

public class UserInfoEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public List<string> Interests { get; set; } = new();
    public string? TravelStyle { get; set; }
    public List<string> Points { get; set; } = new();
    public string? LookingFor { get; set; }
    public List<string> Languages { get; set; } = new();
    public List<string> PersonalityType { get; set; } = new();
    public int Age { get; set; }
    public List<string> Genders { get; set; } = new();
    public string? Destination { get; set; }
    public List<string> Point { get; set; } = new();
    public DateTime? JourneyDate { get; set; }
    public List<string> ChoosenActivity { get; set; } = new();
    public List<string> ChoosenTrip { get; set; } = new();
    public List<string> MoneyAmount { get; set; } = new();
}
