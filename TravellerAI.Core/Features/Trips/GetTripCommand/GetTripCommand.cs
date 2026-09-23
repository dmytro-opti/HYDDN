using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Trips.GetTripCommand;

public class GetTripCommand : IRequest<TripModel>
{
    public Guid UserId { get; set; }
    public Guid TripId { get; set; }
}
