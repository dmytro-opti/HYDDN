using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Trips.GetTripCommand;

public class GetTripCommandHandler : IRequestHandler<GetTripCommand, TripModel>
{
    private readonly ITripService _tripService;

    public GetTripCommandHandler(ITripService tripService)
    {
        _tripService = tripService;
    }

    public async Task<TripModel> Handle(GetTripCommand command, CancellationToken cancellationToken)
    {
        return await _tripService.GetTripAsync(command.UserId, command.TripId);
    }
}
