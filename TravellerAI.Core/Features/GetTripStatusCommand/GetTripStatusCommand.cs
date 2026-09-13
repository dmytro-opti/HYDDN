using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetTripStatusCommand;

public class GetTripStatusCommand : IRequest<TripStatusModel>
{
    public Guid TripId  { get; set; }
}