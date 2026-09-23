using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.GetJourneyProgressCommand;

/// <summary>
/// Completed setup steps and what is still missing.
/// </summary>
public class GetJourneyProgressCommand : IRequest<JourneyProgressModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
}
