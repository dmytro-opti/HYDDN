using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class MapService : IMapService
{
    public Task<MapModel> CreateMapAsync()
    {
        throw new NotImplementedException();
    }

    public Task<MapModel> CreateMapAsync(LocationModel origin, LocationModel destination)
    {
        throw new NotImplementedException();
    }

    public Task<string> BuildOptimalWayAsync(MapModel model)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetAvailableLocationAsync(MapModel model)
    {
        throw new NotImplementedException();
    }

    public Task<string> SelectLocationAsync(MapModel model)
    {
        throw new NotImplementedException();
    }
}