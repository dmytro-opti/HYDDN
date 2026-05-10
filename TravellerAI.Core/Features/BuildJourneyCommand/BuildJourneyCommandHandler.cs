using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildJourneyCommandHandler : IRequestHandler<BuildJourneyCommand, int>
{
    private readonly IJourneyService _journeyService;
    
    public BuildJourneyCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }
    public async Task<int> Handle(BuildJourneyCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.CreateJourney(command);
    }
}