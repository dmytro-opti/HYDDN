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

    public Task<JourneyStatus> Handle(GetJourneyStatusCommand command, CancellationToken cancellationToken)
    {
        // throws NotFoundException when the journey does not exist
        return _journeyService.GetJourneyStatusAsync(command.JourneyId);
    }
}
