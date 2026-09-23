using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Trips.UpdateTripCommand;

public class UpdateTripCommand : IRequest<TripModel>
{
    public Guid UserId { get; set; }
    public Guid TripId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public bool Optimize { get; set; }
    public List<TripStopDraftModel> Stops { get; set; } = new();
}
