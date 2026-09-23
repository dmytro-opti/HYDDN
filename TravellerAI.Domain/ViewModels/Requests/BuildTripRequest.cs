namespace TravellerAI.Domain.ViewModels.Requests;

public class BuildTripRequest
{
    public PeriodViewModel Period { get; set; }
    /// <summary>Trip budget limit.</summary>
    public int Budget { get; set; }
    /// <summary>Journeys settings: period, members, budget and transport.</summary>
    public List<JourneyViewModel>? Journeys { get; set; }
    /// <summary>Default transport for journeys without their own transport.</summary>
    public TransportViewModel? Transport { get; set; }
}
