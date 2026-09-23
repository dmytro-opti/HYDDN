namespace TravellerAI.Domain.ViewModels.Requests;

public class SelectBookingRequest
{
    public Guid TripId { get; set; }
    public Guid UserId { get; set; }
}
