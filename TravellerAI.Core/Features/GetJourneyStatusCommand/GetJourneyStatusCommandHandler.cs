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
        // throws NotFoundException / ForbiddenException
        await _journeyService.EnsureOwnerAsync(command.JourneyId, command.UserId);

        return await _journeyService.GetJourneyStatusAsync(command.JourneyId);
    }
}
