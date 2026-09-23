using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.GetMyJourneysCommand;

/// <summary>
/// Journeys of the user: drafts (Approved = false) can be continued.
/// </summary>
public class GetMyJourneysCommand : IRequest<List<JourneyModel>>
{
    public Guid UserId { get; set; }
    public bool? Approved { get; set; }
}
