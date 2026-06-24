using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

public class JourneyModel
{
    public JourneyStatus Status { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public UserModel User { get; set; }
    public IEnumerable<TripModel> Trips { get; set; }
}

