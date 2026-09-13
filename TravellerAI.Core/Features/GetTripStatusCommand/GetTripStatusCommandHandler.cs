using MediatR;
using MediatR.Pipeline;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetTripStatusCommand;

public class GetTripStatusCommandHandler : IRequestHandler<GetTripStatusCommand, TripStatusModel>
{
    public readonly ITripService _tripService;
    
    public GetTripStatusCommandHandler(ITripService tripService)
    {
        _tripService = tripService;
    }
    
    public async Task<TripStatusModel> Handle(GetTripStatusCommand command, CancellationToken cancellationToken)
    {
        var trip = await _tripService.GetTripAsync(command.TripId);
        if (trip == null)
        {
            throw new Exception($"Trip with id {command.TripId} not found");
        }

        return await _tripService.GetTripStatus(trip.TripId);
    }
}