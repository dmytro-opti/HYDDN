using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.GetJourneyCommand;

public class GetJourneyCommandHandler : IRequestHandler<GetJourneyCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public GetJourneyCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(GetJourneyCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.GetJourneyAsync(command.UserId, command.JourneyId);
    }
}
