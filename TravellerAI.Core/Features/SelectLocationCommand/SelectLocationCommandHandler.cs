using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.SelectLocationCommand;

public class SelectLocationCommandHandler : IRequestHandler<SelectLocationCommand, LocationModel>
{
    private readonly ILocationService _locationService;
    
    public SelectLocationCommandHandler(ILocationService locationService)
    {
        _locationService = locationService;
    }

    public async Task<LocationModel> Handle(SelectLocationCommand command, CancellationToken cancellationToken)
    {
        var location = await _locationService.SelectLocationAsync(command.LocationId);
        if (location == null)
        {
            throw new Exception($"Location {command.LocationId} does not exist");
        }
        return location;
    }
}