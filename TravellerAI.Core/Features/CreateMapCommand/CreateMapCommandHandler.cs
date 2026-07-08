using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.CreateMapCommand;

public class CreateMapCommandHandler : IRequestHandler<CreateMapCommand, MapModel>
{
    private readonly IMapService _mapService;
    
    public CreateMapCommandHandler(IMapService mapService)
    {
        _mapService = mapService;
    }
    public async Task<MapModel> Handle(CreateMapCommand command, CancellationToken cancellationToken)
    {
        var map = await _mapService.CreateMapAsync(command.Origin, command.Destination);
        return map;
    }
}