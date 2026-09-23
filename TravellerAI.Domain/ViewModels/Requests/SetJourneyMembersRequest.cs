namespace TravellerAI.Domain.ViewModels.Requests;

public class SetJourneyMembersRequest
{
    public List<string> Members { get; set; } = new();
}
