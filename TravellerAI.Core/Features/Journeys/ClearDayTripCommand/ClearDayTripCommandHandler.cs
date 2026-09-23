using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.ClearDayTripCommand;

public class ClearDayTripCommandHandler : IRequestHandler<ClearDayTripCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public ClearDayTripCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(ClearDayTripCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.ClearDayTripAsync(command.UserId, command.JourneyId, command.Date);
    }
}
