using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Core.Features.GetJourneyStatusCommand;

public class GetJourneyStatusCommandHandler : IRequestHandler<GetJourneyStatusCommand, JourneyStatus>
{
    private readonly IJourneyService _journeyService;
    
    public GetJourneyStatusCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyStatus> Handle(GetJourneyStatusCommand command, CancellationToken cancellationToken)
    {
        var journey = _journeyService.GetJourneyAsync(command.journeyId);
        if (journey == null)
        {
            throw new Exception($"Journey {command.journeyId} not found");
        }

        return await _journeyService.GetJourneyStatusAsync(command.journeyId);
    }
}