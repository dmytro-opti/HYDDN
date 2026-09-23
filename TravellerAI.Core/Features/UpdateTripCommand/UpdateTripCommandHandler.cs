using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.UpdateTripCommand;

public class UpdateTripCommandHandler : IRequestHandler<UpdateTripCommand, TripModel>
{
    private readonly ITripService _tripService;

    public UpdateTripCommandHandler(ITripService tripService)
    {
        _tripService = tripService;
    }

    public async Task<TripModel> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _tripService.GetTripAsync(request.TripId);
        if (trip == null)
        {
            throw new Exception($"Trip {request.TripId} does not exist");
        }

        trip.Name = request.Name;
        trip.Group = request.Group;
        trip.Booking = request.Booking;
        trip.Map = request.Map;
        trip.Period = request.Period;
        trip.Rating = request.Rating;
        await _tripService.UpdateTripAsync(trip);
        return trip;
    }
}