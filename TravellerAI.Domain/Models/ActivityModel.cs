using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

public class ActivityModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ActivityType Type { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public LocationModel Location { get; set; }
    public JourneyStatus Status { get; set; }
    public decimal Rating { get; set; }
    public string ImageUrl { get; set; }
    public string VideoUrl { get; set; }
    public ReviewModel Review { get; set; }
    public PeriodModel Period { get; set; }
}