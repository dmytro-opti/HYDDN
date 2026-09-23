namespace TravellerAI.Domain.ViewModels;

public class JourneyDayViewModel
{
    public DateTime Date { get; set; }
    public Guid? TripId { get; set; }
    public string? TripName { get; set; }
    /// <summary>The day trip has to start here (hotel of the previous night).</summary>
    public Guid? StartLocationId { get; set; }
    /// <summary>The day trip has to finish here (hotel of the coming night).</summary>
    public Guid? EndLocationId { get; set; }
    public bool IsHotelSwitch { get; set; }
}
