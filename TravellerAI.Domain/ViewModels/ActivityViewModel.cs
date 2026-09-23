using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class ActivityViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ActivityType Type { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public decimal Rating { get; set; }
    public string ImageUrl { get; set; }
    public LocationViewModel Location { get; set; }
}
