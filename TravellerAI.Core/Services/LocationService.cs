using AutoMapper;
using TravellerAI.Core.Interfaces;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class LocationService : ILocationService
{
    private readonly IRepository<LocationEntity> _locationRepository;
    private readonly IMapper _mapper;

    public LocationService(IRepository<LocationEntity> locationRepository, IMapper mapper)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns locations of the country, optionally narrowed to the city.
    /// Comparison is case-insensitive (default SQL Server collation).
    /// </summary>
    public async Task<IEnumerable<LocationModel>> GetLocationByCountryAndCity(string country, string? city)
    {
        country = country.Trim();
        city = string.IsNullOrWhiteSpace(city) ? null : city.Trim();

        var locations = await _locationRepository.FindAsync(l =>
            l.Country == country && (city == null || l.City == city));

        return _mapper.Map<List<LocationModel>>(locations
            .OrderBy(l => l.City)
            .ThenBy(l => l.Street));
    }
}
