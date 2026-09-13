using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetTripsCommand;

public class GetTripsCommandHandler : IRequestHandler<GetTripsCommand, IEnumerable<TripModel>>
{
    private readonly ITripService _tripService;

    public GetTripsCommandHandler(ITripService tripService)
    {
        _tripService = tripService;
    }

    public async Task<IEnumerable<TripModel>> Handle(GetTripsCommand command, CancellationToken cancellationToken)
    {
        var trips = await _tripService.GetTripsAsync(command.UserId, command.Period, command.PriceFrom, command.PriceTo, command.LocationId);
        return trips;
    }
}