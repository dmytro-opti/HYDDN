using MediatR;

namespace TravellerAI.Core.Features.Journeys.DeleteJourneyCommand;

/// <summary>
/// Deletes a not approved journey and cancels its hotel bookings.
/// </summary>
public class DeleteJourneyCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
}
