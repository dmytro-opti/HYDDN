using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Catalog.GetCountryLocationsCommand;

public class GetCountryLocationsCommandHandler : IRequestHandler<GetCountryLocationsCommand, List<LocationModel>>
{
    private readonly IMapService _mapService;

    public GetCountryLocationsCommandHandler(IMapService mapService)
    {
        _mapService = mapService;
    }

    public async Task<List<LocationModel>> Handle(GetCountryLocationsCommand command, CancellationToken cancellationToken)
    {
        return (await _mapService.GetAvailableLocationsAsync(command.CountryId, command.City)).ToList();
    }
}
