namespace TravellerAI.Domain.ViewModels.Requests;

/// <summary>
/// Step 1: journey details. The journey is created as a draft.
/// </summary>
public class CreateJourneyRequest
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public List<string>? Members { get; set; }
}
