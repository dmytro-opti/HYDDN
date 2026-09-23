using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetJourneyStatusCommand;

public class GetJourneyStatusCommand : IRequest<JourneyStatus>
{
    public Guid JourneyId{get;set;}
    public Guid UserId { get; set; }
}