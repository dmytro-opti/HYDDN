using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

/// <summary>
/// Routes between locations (one day trips).
/// </summary>
public interface IMapService
{
    /// <summary>Creates a route through the locations in the given order.</summary>
    Task<MapModel> CreateMapAsync(IEnumerable<Guid> locationIds);

    /// <summary>Reorders route points (keeping the start point and optionally the last one) to minimize the total distance.</summary>
    Task<MapModel> BuildOptimalWayAsync(MapModel model, bool keepLastPoint = false);

    /// <summary>Locations of the country (optionally city) which can be placed on a map.</summary>
    Task<IEnumerable<LocationModel>> GetAvailableLocationsAsync(Guid countryId, string? city = null);

    /// <summary>Appends the location to the route.</summary>
    Task<MapModel> SelectLocationAsync(MapModel model, Guid locationId);
}
