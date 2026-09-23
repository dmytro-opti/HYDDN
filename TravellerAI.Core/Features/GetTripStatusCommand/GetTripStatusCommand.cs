using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetTripStatusCommand;

public class GetTripStatusCommand : IRequest<TripStatus>
{
    public Guid TripId  { get; set; }
}