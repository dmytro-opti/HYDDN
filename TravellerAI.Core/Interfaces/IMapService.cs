using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface IMapService
{
    Task<MapModel> CreateMapAsync();
    Task<string> BuildOptimalWayAsync(MapModel model);
    Task<string> GetAvailableLocationAsync(MapModel model);
    Task<string> SelectLocationAsync(MapModel model);
}