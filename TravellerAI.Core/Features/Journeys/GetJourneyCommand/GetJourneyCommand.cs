using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.GetJourneyCommand;

public class GetJourneyCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
}
