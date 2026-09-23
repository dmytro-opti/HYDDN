using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.AssignDayTripCommand;

public class AssignDayTripCommandHandler : IRequestHandler<AssignDayTripCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public AssignDayTripCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(AssignDayTripCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.AssignDayTripAsync(command.UserId, command.JourneyId, command.Date, command.TripId);
    }
}
