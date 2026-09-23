using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Trips.CreateTripCommand;

/// <summary>
/// Creates a trip route: ordered locations / activities in one city.
/// </summary>
public class CreateTripCommand : IRequest<TripModel>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public bool Optimize { get; set; }
    public List<TripStopDraftModel> Stops { get; set; } = new();
}
