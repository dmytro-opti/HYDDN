using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.SetJourneyMembersCommand;

public class SetJourneyMembersCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public List<string> Members { get; set; } = new();
}
