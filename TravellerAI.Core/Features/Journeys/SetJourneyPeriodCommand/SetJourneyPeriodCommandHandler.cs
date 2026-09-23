using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.SetJourneyPeriodCommand;

public class SetJourneyPeriodCommandHandler : IRequestHandler<SetJourneyPeriodCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public SetJourneyPeriodCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(SetJourneyPeriodCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.SetPeriodAsync(command.UserId, command.JourneyId, command.Start, command.End);
    }
}
