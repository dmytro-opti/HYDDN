using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.GetDayTripsCommand;

public class GetDayTripsCommandHandler : IRequestHandler<GetDayTripsCommand, List<TripModel>>
{
    private readonly IJourneyService _journeyService;

    public GetDayTripsCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<List<TripModel>> Handle(GetDayTripsCommand command, CancellationToken cancellationToken)
    {
        return (await _journeyService.GetAvailableDayTripsAsync(command.UserId, command.JourneyId, command.Date)).ToList();
    }
}
