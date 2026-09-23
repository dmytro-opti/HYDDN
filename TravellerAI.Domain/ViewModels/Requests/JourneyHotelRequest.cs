namespace TravellerAI.Domain.ViewModels.Requests;

/// <summary>
/// Step 4: hotel stay. Stays have to cover every night of the journey; several stays mean hotel switches.
/// </summary>
public class JourneyHotelRequest
{
    public Guid PlaceId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
}
