using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.UnapproveJourneyCommand;

/// <summary>
/// Enables editing of an approved journey (before it starts).
/// </summary>
public class UnapproveJourneyCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
}
