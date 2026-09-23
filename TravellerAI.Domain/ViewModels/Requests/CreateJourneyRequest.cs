namespace TravellerAI.Domain.ViewModels.Requests;

public class CreateJourneyRequest
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public PeriodViewModel? Period { get; set; }
}
