using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features;

public class BuildTripCommandHandler : IRequestHandler<BuildTripCommand, int>
{
    private readonly ITripService _tripService;

    public BuildTripCommandHandler(ITripService tripService)
    {
        _tripService = tripService;
    }

    public async Task<int> Handle(BuildTripCommand command, CancellationToken cancellationToken)
    {
        return await _tripService.CreateTrip(command);
    }
    
}