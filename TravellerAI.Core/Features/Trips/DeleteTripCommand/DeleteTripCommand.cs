using MediatR;

namespace TravellerAI.Core.Features.Trips.DeleteTripCommand;

public class DeleteTripCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public Guid TripId { get; set; }
}
