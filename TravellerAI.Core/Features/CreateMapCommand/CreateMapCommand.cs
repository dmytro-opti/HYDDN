using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.CreateMapCommand;

public class CreateMapCommand : IRequest<MapModel>
{
    public LocationModel Origin { get; set; }
    public LocationModel Destination { get; set; }
}