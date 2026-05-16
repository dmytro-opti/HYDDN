using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildTripCommand : IRequest<Guid>
{
    public string Name  { get; set; }
    public UserModel User { get; set; }
    public GroupModel Group { get; set; }
    public PeriodModel Period { get; set; }
}