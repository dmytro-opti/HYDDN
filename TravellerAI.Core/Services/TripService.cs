using AutoMapper;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Geo;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using static TravellerAI.Core.Constants;

namespace TravellerAI.Core.Services;

public class TripService : ITripService
{
    private readonly ITripRepository _tripRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRepository<LocationEntity> _locationRepository;
    private readonly IRepository<ActivityEntity> _activityRepository;
    private readonly IMapService _mapService;
    private readonly ILoggerService<TripService> _logger;
    private readonly IMapper _mapper;

    public TripService(ITripRepository tripRepository, IUserRepository userRepository, IRepository<LocationEntity> locationRepository,
        IRepository<ActivityEntity> activityRepository, IMapService mapService, ILoggerService<TripService> logger, IMapper mapper)
    {
        _tripRepository = tripRepository;
        _userRepository = userRepository;
        _locationRepository = locationRepository;
        _activityRepository = activityRepository;
        _mapService = mapService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TripModel> CreateTripAsync(Guid userId, TripDraftModel draft)
    {
        if (!await _userRepository.ExistsAsync(userId))
        {
            throw new NotFoundException("User", userId);
        }

        var stops = await BuildStopsAsync(draft);
        var trip = new TripEntity { UserId = userId };
        Apply(trip, draft, stops);

        await _tripRepository.AddAsync(trip);
        _logger.Log(ErrorLevel.Low, $"Trip {trip.Id} was created by user {userId}");

        return _mapper.Map<TripModel>(trip);
    }

    public async Task<TripModel> UpdateTripAsync(Guid userId, Guid tripId, TripDraftModel draft)
    {
        var trip = await GetOwnTripAsync(tripId, userId);

        if (await _tripRepository.IsUsedByOthersOrApprovedAsync(tripId, userId))
        {
            throw new ConflictException($"Trip {tripId} is used by other travellers or approved journeys, create a new trip instead");
        }

        var stops = await BuildStopsAsync(draft);

        trip.Stops.Clear();
        Apply(trip, draft, stops);
        await _tripRepository.UpdateAsync(trip);

        return _mapper.Map<TripModel>(trip);
    }

    public async Task DeleteTripAsync(Guid userId, Guid tripId)
    {
        await GetOwnTripAsync(tripId, userId);

        if (await _tripRepository.IsUsedAsync(tripId))
        {
            throw new ConflictException($"Trip {tripId} is scheduled in journeys and cannot be deleted");
        }

        await _tripRepository.DeleteAsync(tripId);
        _logger.Log(ErrorLevel.Low, $"Trip {tripId} was deleted");
    }

    public async Task<TripModel> GetTripAsync(Guid userId, Guid tripId)
    {
        var trip = await GetTripEntityAsync(tripId);

        if (!trip.IsPublic && trip.UserId != userId)
        {
            throw new ForbiddenException($"Trip {tripId} is private");
        }

        return _mapper.Map<TripModel>(trip);
    }

    public async Task<IReadOnlyList<TripModel>> SearchTripsAsync(Guid userId, Guid countryId, string? city, Guid? startLocationId,
        Guid? endLocationId)
    {
        city = string.IsNullOrWhiteSpace(city) ? null : city.Trim();
        var trips = await _tripRepository.SearchCatalogAsync(countryId, city, userId);

        return trips
            .Where(t => t.Stops.Count > 0)
            .Where(t => startLocationId == null || t.Stops.MinBy(s => s.Order)!.LocationId == startLocationId)
            .Where(t => endLocationId == null || t.Stops.MaxBy(s => s.Order)!.LocationId == endLocationId)
            .OrderByDescending(t => t.Rating)
            .ThenBy(t => t.DistanceKm)
            .Select(t => _mapper.Map<TripModel>(t))
            .ToList();
    }

    private static void Apply(TripEntity trip, TripDraftModel draft, IReadOnlyList<ResolvedStop> stops)
    {
        trip.Name = draft.Name.Trim();
        trip.Description = string.IsNullOrWhiteSpace(draft.Description) ? null : draft.Description.Trim();
        trip.IsPublic = draft.IsPublic;
        trip.CountryId = stops[0].Location.CountryId;
        trip.City = stops[0].Location.City!.Trim();
        trip.DistanceKm = Math.Round(stops.Sum(s => s.DistanceFromPreviousKm), 2);

        for (var i = 0; i < stops.Count; i++)
        {
            trip.Stops.Add(new TripStopEntity
            {
                Order = i,
                LocationId = stops[i].Location.Id,
                ActivityId = stops[i].Activity?.Id,
                DistanceFromPreviousKm = stops[i].DistanceFromPreviousKm
            });
        }
    }

    /// <summary>
    /// Resolves stops to locations, validates them and (optionally) optimizes their order.
    /// </summary>
    private async Task<IReadOnlyList<ResolvedStop>> BuildStopsAsync(TripDraftModel draft)
    {
        if (draft.Stops.Count is < Journey.MinTripStops or > Journey.MaxTripStops)
        {
            throw new BadRequestException($"Trip must have {Journey.MinTripStops}-{Journey.MaxTripStops} stops");
        }

        var stops = new List<ResolvedStop>();
        foreach (var stop in draft.Stops)
        {
            stops.Add(await ResolveStopAsync(stop));
        }

        EnsureSameCountryAndCity(stops);
        EnsureUniqueLocations(stops);

        if (draft.Optimize && stops.Count > 3)
        {
            stops = await OptimizeAsync(stops);
        }

        CalculateAndValidateDistances(stops);

        return stops;
    }

    private async Task<ResolvedStop> ResolveStopAsync(TripStopDraftModel stop)
    {
        LocationEntity? location;
        ActivityEntity? activity = null;

        if (stop.ActivityId.HasValue)
        {
            activity = await _activityRepository.GetByIdAsync(stop.ActivityId.Value)
                       ?? throw new NotFoundException("Activity", stop.ActivityId.Value);
            // activity address is the stop location
            location = activity.Location ?? throw new BadRequestException($"Activity '{activity.Name}' has no address");
        }
        else if (stop.LocationId.HasValue)
        {
            location = await _locationRepository.GetByIdAsync(stop.LocationId.Value)
                       ?? throw new NotFoundException("Location", stop.LocationId.Value);
        }
        else
        {
            throw new BadRequestException("Every stop needs a location or an activity");
        }

        if (location.Latitude == null || location.Longitude == null)
        {
            throw new BadRequestException($"Location '{location.Name ?? location.Street}' has no coordinates");
        }

        return new ResolvedStop(location, activity);
    }

    private static void EnsureSameCountryAndCity(IReadOnlyList<ResolvedStop> stops)
    {
        var first = stops[0].Location;

        if (string.IsNullOrWhiteSpace(first.City))
        {
            throw new BadRequestException($"Location '{first.Name ?? first.Street}' has no city");
        }

        foreach (var stop in stops.Skip(1))
        {
            if (stop.Location.CountryId != first.CountryId)
            {
                throw new BadRequestException($"All stops must be in one country: '{stop.Name}' is in another country");
            }

            if (!string.Equals(stop.Location.City?.Trim(), first.City.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                throw new BadRequestException($"All stops must be in {first.City}: '{stop.Name}' is in {stop.Location.City}");
            }
        }
    }

    /// <summary>
    /// A location is visited once; only the first and the last stop (hotel) can be the same.
    /// </summary>
    private static void EnsureUniqueLocations(IReadOnlyList<ResolvedStop> stops)
    {
        for (var i = 1; i < stops.Count; i++)
        {
            if (stops[i].Location.Id == stops[i - 1].Location.Id)
            {
                throw new BadRequestException($"Stops {i} and {i + 1} are the same location '{stops[i].Name}'");
            }
        }

        var middle = stops.Skip(1).Take(stops.Count - 2).Select(s => s.Location.Id).ToList();
        var endpoints = new[] { stops[0].Location.Id, stops[^1].Location.Id };

        if (middle.Count != middle.Distinct().Count() || middle.Any(endpoints.Contains))
        {
            throw new BadRequestException("Each location can be visited once (only the start and the finish can be the same)");
        }
    }

    /// <summary>
    /// Reorders the stops between the fixed first and last stop by the shortest route.
    /// </summary>
    private async Task<List<ResolvedStop>> OptimizeAsync(List<ResolvedStop> stops)
    {
        var map = await _mapService.CreateMapAsync(stops.Select(s => s.Location.Id));
        var optimized = await _mapService.BuildOptimalWayAsync(map, keepLastPoint: true);

        var middle = stops.Skip(1).Take(stops.Count - 2).ToDictionary(s => s.Location.Id);
        var ordered = optimized.Points.Skip(1).Take(optimized.Points.Count - 2).Select(p => middle[p.LocationId]);

        return new[] { stops[0] }.Concat(ordered).Append(stops[^1]).ToList();
    }

    private static void CalculateAndValidateDistances(IReadOnlyList<ResolvedStop> stops)
    {
        for (var i = 1; i < stops.Count; i++)
        {
            var previous = stops[i - 1].Location;
            var current = stops[i].Location;
            var distance = Math.Round(GeoCalculator.DistanceKm(previous.Latitude!.Value, previous.Longitude!.Value,
                current.Latitude!.Value, current.Longitude!.Value), 2);

            if (distance > Journey.MaxStopDistanceKm)
            {
                throw new BadRequestException(
                    $"'{stops[i - 1].Name}' and '{stops[i].Name}' are {distance} km apart, max {Journey.MaxStopDistanceKm} km between stops");
            }

            stops[i].DistanceFromPreviousKm = distance;
        }

        var total = stops.Sum(s => s.DistanceFromPreviousKm);
        if (total > Journey.MaxTripDistanceKm)
        {
            throw new BadRequestException($"Trip is {Math.Round(total, 2)} km long, max {Journey.MaxTripDistanceKm} km per day");
        }
    }

    private async Task<TripEntity> GetTripEntityAsync(Guid tripId)
    {
        var trip = await _tripRepository.GetByIdAsync(tripId);

        if (trip == null)
        {
            _logger.Log(ErrorLevel.Medium, $"Trip {tripId} not found");
            throw new NotFoundException("Trip", tripId);
        }

        return trip;
    }

    private async Task<TripEntity> GetOwnTripAsync(Guid tripId, Guid userId)
    {
        var trip = await GetTripEntityAsync(tripId);

        if (trip.UserId != userId)
        {
            throw new ForbiddenException($"Trip {tripId} can be changed only by its author");
        }

        return trip;
    }

    private sealed class ResolvedStop
    {
        public ResolvedStop(LocationEntity location, ActivityEntity? activity)
        {
            Location = location;
            Activity = activity;
        }

        public LocationEntity Location { get; }
        public ActivityEntity? Activity { get; }
        public double DistanceFromPreviousKm { get; set; }
        public string Name => Activity?.Name ?? Location.Name ?? Location.Street ?? Location.Id.ToString();
    }
}
