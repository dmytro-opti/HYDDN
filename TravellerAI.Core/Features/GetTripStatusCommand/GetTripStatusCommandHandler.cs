using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Core.Features.GetTripStatusCommand;

public class GetTripStatusCommandHandler : IRequestHandler<GetTripStatusCommand, TripStatus>
{
    private readonly ITripService _tripService;
    
    public GetTripStatusCommandHandler(ITripService tripService)
    {
        _tripService = tripService;
    }
    
    public Task<TripStatus> Handle(GetTripStatusCommand command, CancellationToken cancellationToken)
    {
        // throws NotFoundException when the trip does not exist
        return _tripService.GetTripStatusAsync(command.TripId);
    }
}
