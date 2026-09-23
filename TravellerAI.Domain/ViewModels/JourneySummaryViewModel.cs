using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class JourneySummaryViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public JourneyStatus Status { get; set; }
    public bool Approved { get; set; }
    public PeriodViewModel Period { get; set; }
    public string Country { get; set; }
}
