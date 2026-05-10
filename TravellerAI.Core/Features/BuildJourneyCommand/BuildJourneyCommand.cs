using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildJourneyCommand : IRequest<int>
{
    public PeriodModel Period { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}