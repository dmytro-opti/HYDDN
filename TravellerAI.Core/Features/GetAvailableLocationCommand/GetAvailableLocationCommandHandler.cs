using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetAvailableLocationCommand;

public class GetAvailableLocationCommandHandler : IRequestHandler<GetAvailableLocationListCommand, IEnumerable<LocationModel>>
{
    private readonly ILocationService _locationService; 
    public GetAvailableLocationCommandHandler(ILocationService locationService)
    {
        _locationService = locationService;
    }
    public async Task<IEnumerable<LocationModel>> Handle(GetAvailableLocationListCommand command, CancellationToken cancellationToken)
    {
        var locations = await _locationService.GetLocationByCountryAndCity(command.Country, command.City);
        if (locations == null)
        {
            return Enumerable.Empty<LocationModel>();
        }
        return locations;
    }
}