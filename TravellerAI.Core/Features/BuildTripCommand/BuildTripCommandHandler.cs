using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.BuildTripCommand;

public class BuildTripCommandHandler : IRequestHandler<BuildTripCommand, Guid>
{
    private readonly ITripService _tripService;

    public BuildTripCommandHandler(ITripService tripService)
    {
        _tripService = tripService;
    }

    public async Task<Guid> Handle(BuildTripCommand command, CancellationToken cancellationToken)
    {
        return await _tripService.CreateTrip(command);
    }
    
}