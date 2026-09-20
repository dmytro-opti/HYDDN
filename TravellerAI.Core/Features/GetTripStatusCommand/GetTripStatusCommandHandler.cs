using MediatR;
using MediatR.Pipeline;
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
    
    public async Task<TripStatus> Handle(GetTripStatusCommand command, CancellationToken cancellationToken)
    {
        var tripStatus = await _tripService.GetTripStatusAsync(command.TripId);
        if (tripStatus == null)
        {
            throw new Exception($"Trip with id {command.TripId} not found");
        }
        return tripStatus;
    }
}