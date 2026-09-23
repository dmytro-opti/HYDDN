using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildJourneyCommandHandler : IRequestHandler<BuildJourneyCommand, Guid>
{
    private readonly IJourneyService _journeyService;
    
    public BuildJourneyCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public Task<Guid> Handle(BuildJourneyCommand command, CancellationToken cancellationToken)
    {
        // user existence is checked by the service
        return _journeyService.CreateJourney(command);
    }
}
