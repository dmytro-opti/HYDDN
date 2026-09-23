using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.ApproveJourneyCommand;

public class ApproveJourneyCommandHandler : IRequestHandler<ApproveJourneyCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public ApproveJourneyCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(ApproveJourneyCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.ApproveAsync(command.UserId, command.JourneyId);
    }
}
