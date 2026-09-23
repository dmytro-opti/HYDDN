using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Geo;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class MapService : IMapService
{
    private readonly IRepository<LocationEntity> _locationRepository;
    private readonly IMapper _mapper;

    public MapService(IRepository<LocationEntity> locationRepository, IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    public async Task<MapModel> CreateMapAsync(IEnumerable<Guid> locationIds)
    {
        // a location can repeat, e.g. the hotel at the start and at the finish of a day trip
        var ids = locationIds.ToList();
        var distinctIds = ids.Distinct().ToList();
        var locations = await _locationRepository.FindAsync(l => distinctIds.Contains(l.Id));

        var missing = distinctIds.Except(locations.Select(l => l.Id)).ToList();
        if (missing.Count > 0)
        {
            throw new NotFoundException($"Locations not found: {string.Join(", ", missing)}");
        }

        // keep the requested order
        var byId = locations.ToDictionary(l => l.Id);
        var map = new MapModel { Points = ids.Select(id => ToPoint(byId[id])).ToList() };

        return Recalculate(map);
    }

    public Task<MapModel> BuildOptimalWayAsync(MapModel model, bool keepLastPoint = false)
    {
        var fixedPoints = keepLastPoint ? 2 : 1;
        if (model.Points.Count - fixedPoints < 2)
        {
            model.IsOptimized = true;
            return Task.FromResult(Recalculate(model));
        }

        var free = keepLastPoint ? model.Points.Take(model.Points.Count - 1).ToList() : model.Points.ToList();
        var route = NearestNeighbour(free);
        if (keepLastPoint)
        {
            route.Add(model.Points[^1]);
        }

        ImproveWithTwoOpt(route, keepLastPoint);

        model.Points = route;
        model.IsOptimized = true;

        return Task.FromResult(Recalculate(model));
    }

    public async Task<IEnumerable<LocationModel>> GetAvailableLocationsAsync(Guid countryId, string? city = null)
    {
        city = string.IsNullOrWhiteSpace(city) ? null : city.Trim();

        var locations = await _locationRepository.FindAsync(l =>
            l.CountryId == countryId
            && l.Latitude != null && l.Longitude != null
            && (city == null || l.City == city));

        return _mapper.Map<List<LocationModel>>(locations.OrderBy(l => l.City).ThenBy(l => l.Name));
    }

    public async Task<MapModel> SelectLocationAsync(MapModel model, Guid locationId)
    {
        if (model.Points.Any(p => p.LocationId == locationId))
        {
            throw new ConflictException($"Location {locationId} is already on the map");
        }

        var location = await _locationRepository.GetByIdAsync(locationId)
                       ?? throw new NotFoundException("Location", locationId);

        model.Points.Add(ToPoint(location));
        model.IsOptimized = false;

        return Recalculate(model);
    }

    private static PointModel ToPoint(LocationEntity location)
    {
        if (location.Latitude == null || location.Longitude == null)
        {
            throw new BadRequestException($"Location {location.Id} has no coordinates");
        }

        return new PointModel
        {
            LocationId = location.Id,
            Name = location.Name ?? location.Street ?? location.City,
            Latitude = location.Latitude.Value,
            Longitude = location.Longitude.Value
        };
    }

    /// <summary>
    /// Greedy route from the first point: always go to the closest not visited point.
    /// </summary>
    private static List<PointModel> NearestNeighbour(IReadOnlyList<PointModel> points)
    {
        var route = new List<PointModel> { points[0] };
        var left = points.Skip(1).ToList();

        while (left.Count > 0)
        {
            var current = route[^1];
            var next = left.MinBy(p => Distance(current, p))!;
            route.Add(next);
            left.Remove(next);
        }

        return route;
    }

    /// <summary>
    /// 2-opt: reverses route segments while it shortens the open path. The start (and optionally the last) point stays fixed.
    /// </summary>
    private static void ImproveWithTwoOpt(List<PointModel> route, bool keepLastPoint)
    {
        var lastMovable = keepLastPoint ? route.Count - 2 : route.Count - 1;
        var improved = true;
        while (improved)
        {
            improved = false;
            for (var i = 1; i < lastMovable; i++)
            {
                for (var k = i + 1; k <= lastMovable; k++)
                {
                    var before = Distance(route[i - 1], route[i]) + (k + 1 < route.Count ? Distance(route[k], route[k + 1]) : 0);
                    var after = Distance(route[i - 1], route[k]) + (k + 1 < route.Count ? Distance(route[i], route[k + 1]) : 0);

                    if (after + 1e-9 < before)
                    {
                        route.Reverse(i, k - i + 1);
                        improved = true;
                    }
                }
            }
        }
    }

    private static MapModel Recalculate(MapModel map)
    {
        for (var i = 0; i < map.Points.Count; i++)
        {
            map.Points[i].Order = i;
            map.Points[i].DistanceFromPreviousKm = i == 0 ? 0 : Math.Round(Distance(map.Points[i - 1], map.Points[i]), 2);
        }

        map.TotalDistanceKm = Math.Round(map.Points.Sum(p => p.DistanceFromPreviousKm), 2);

        return map;
    }

    private static double Distance(PointModel a, PointModel b) =>
        GeoCalculator.DistanceKm(a.Latitude, a.Longitude, b.Latitude, b.Longitude);
}
