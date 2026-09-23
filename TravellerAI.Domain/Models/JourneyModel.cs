using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

public class JourneyModel
{
    public Guid Id { get; set; }
    public JourneyStatus Status { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PeriodModel Period { get; set; }
    public List<string> Members { get; set; }
    public BudgetModel Budget { get; set; }
    public UserModel User { get; set; }
    public IEnumerable<TripModel> Trips { get; set; }
}

