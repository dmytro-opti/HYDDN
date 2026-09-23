namespace TravellerAI.Domain.ViewModels.Requests;

/// <summary>
/// Step 5: trip of the day. It has to start at the hotel of the previous night and finish at the hotel of the coming night.
/// </summary>
public class AssignDayTripRequest
{
    public Guid TripId { get; set; }
}
