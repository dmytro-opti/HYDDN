using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.GetJourneyProgressCommand;

public class GetJourneyProgressCommandHandler : IRequestHandler<GetJourneyProgressCommand, JourneyProgressModel>
{
    private readonly IJourneyService _journeyService;

    public GetJourneyProgressCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyProgressModel> Handle(GetJourneyProgressCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.GetProgressAsync(command.UserId, command.JourneyId);
    }
}
