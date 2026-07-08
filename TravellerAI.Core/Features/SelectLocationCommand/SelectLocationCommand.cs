using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.SelectLocationCommand;

public class SelectLocationCommand : IRequest<LocationModel>
{
    public Guid LocationId { get; set; }
}