using MediatR;

namespace TravellerAI.Core.Features.Journeys.CreateJourneyCommand;

/// <summary>
/// Step 1: creates a draft journey.
/// </summary>
public class CreateJourneyCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public List<string>? Members { get; set; }
}
