namespace TravellerAI.Domain.ViewModels.Requests;

public class AddBookingRequest
{
    public Guid UserId { get; set; }
    public PeriodViewModel Period { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
}
