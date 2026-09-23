namespace TravellerAI.Domain.ViewModels.Requests;

/// <summary>
/// Step 2: first and last day of the journey (dates, time is ignored). Nights: Start .. End - 1.
/// </summary>
public class SetJourneyPeriodRequest
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
