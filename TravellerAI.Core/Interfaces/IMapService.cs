using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface IMapService
{
    Task<MapModel> CreateMapAsync(LocationModel origin, LocationModel destination);
    Task<string> BuildOptimalWayAsync(MapModel model);
    Task<string> GetAvailableLocationAsync(MapModel model);
    Task<string> SelectLocationAsync(MapModel model);
}