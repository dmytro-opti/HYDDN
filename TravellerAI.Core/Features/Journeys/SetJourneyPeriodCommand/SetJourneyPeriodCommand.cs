using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.SetJourneyPeriodCommand;

/// <summary>
/// Step 2: journey dates.
/// </summary>
public class SetJourneyPeriodCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
