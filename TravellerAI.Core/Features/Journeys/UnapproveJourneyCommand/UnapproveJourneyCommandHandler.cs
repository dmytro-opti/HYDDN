using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.UnapproveJourneyCommand;

public class UnapproveJourneyCommandHandler : IRequestHandler<UnapproveJourneyCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public UnapproveJourneyCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(UnapproveJourneyCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.UnapproveAsync(command.UserId, command.JourneyId);
    }
}
