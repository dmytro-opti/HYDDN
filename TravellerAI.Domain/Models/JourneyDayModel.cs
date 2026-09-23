namespace TravellerAI.Domain.Models;

public class JourneyDayModel
{
    public DateTime Date { get; set; }
    public Guid? TripId { get; set; }
    public string? TripName { get; set; }
    /// <summary>Location the day trip has to start from (hotel of the previous night).</summary>
    public Guid? StartLocationId { get; set; }
    /// <summary>Location the day trip has to finish at (hotel of the coming night).</summary>
    public Guid? EndLocationId { get; set; }
    /// <summary>The traveller moves from one hotel to another on this day.</summary>
    public bool IsHotelSwitch { get; set; }
}
