using MediatR;
using Microsoft.Extensions.Logging;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetTripsCommand;

public class GetTripsCommandHandler : IRequestHandler<GetTripsCommand, IEnumerable<TripModel>>
{
    private readonly ITripService _tripService;
    private readonly ILogger<GetTripsCommandHandler> _logger;

    public GetTripsCommandHandler(ITripService tripService, ILogger<GetTripsCommandHandler> logger)
    {
        _tripService = tripService;
        _logger = logger;
    }

    public async Task<IEnumerable<TripModel>> Handle(GetTripsCommand command, CancellationToken cancellationToken)
    {
        var trips = await _tripService.GetTripsAsync(command.UserId, command.Period, command.PriceFrom, command.PriceTo, command.LocationId);
        _logger.Log(LogLevel.Information, "Retrieved {Count} trips for user {UserId}", trips.Count(), command.UserId);
        return trips;
    }
}