using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.BuildOptimalWayCommand;

public class BuildOptimalWayCommandHandler : IRequestHandler<BuildOptimalWayCommand, OptimalWayModel>
{
    private readonly ITripService _tripService;
    private readonly IOptimalWayService _optimalWayService;
    
    public BuildOptimalWayCommandHandler(ITripService tripService, IOptimalWayService optimalWayService)
    {
        _tripService = tripService;
        _optimalWayService = optimalWayService;
    }
    public async Task<OptimalWayModel> Handle(BuildOptimalWayCommand command, CancellationToken cancellationToken)
    {
        var trip = await _tripService.GetTripAsync(command.TripId);
        if (trip == null)
        {
            throw new Exception($"Trip with ID {command.TripId} not found.");
        }

        var optimalWay = new OptimalWayModel
        {
            TripId = command.TripId,
            StartPoint = command.StartPoint,
            EndPoint = command.EndPoint,
            OptimizationType = command.OptimizationType,
            TravelMode = command.TravelMode,
            Waypoints = command.Waypoints
        };

        var result = await _optimalWayService.BuildOptimalWayAsync(optimalWay);
        return result;
    }
}