using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class LocationService : ILocationService
{
    public Task<IEnumerable<LocationModel>> GetLocationByCountryAndCity(string country, string? city)
    {
        throw new NotImplementedException();
    }
}