using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Trips.SearchTripsCommand;

public class SearchTripsCommandHandler : IRequestHandler<SearchTripsCommand, List<TripModel>>
{
    private readonly ITripService _tripService;

    public SearchTripsCommandHandler(ITripService tripService)
    {
        _tripService = tripService;
    }

    public async Task<List<TripModel>> Handle(SearchTripsCommand command, CancellationToken cancellationToken)
    {
        return (await _tripService.SearchTripsAsync(command.UserId, command.CountryId, command.City, command.StartLocationId, command.EndLocationId)).ToList();
    }
}
