using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.ApproveJourneyCommand;

/// <summary>
/// Finishes the setup: every step is validated, hotel bookings are frozen, edits are disabled.
/// </summary>
public class ApproveJourneyCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
}
