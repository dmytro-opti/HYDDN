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
        var status = await _journeyService.GetJourneyStatusAsync(command.JourneyId);
        if (status == null)
        {
            throw new Exception($"Journey with id {command.JourneyId} not found");
        }
        return status;
    }
}