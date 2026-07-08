using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.BuildOptimalWayCommand;

public class BuildOptimalWayCommand : IRequest<OptimalWayModel>
{
    public Guid TripId { get; set; }
    public LocationModel StartPoint { get; set; }
    public LocationModel EndPoint { get; set; }
    public RouteOptimizationType OptimizationType { get; set; }
    public TravelMode TravelMode { get; set; }
    public List<LocationModel> Waypoints { get; set; } = new();
}