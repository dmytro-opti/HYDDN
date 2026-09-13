using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetAvailableLocationCommand;

public class GetAvailableLocationListCommand : IRequest<IEnumerable<LocationModel>>
{
    public string Country { get; set; }
    public string? City { get; set; }
}