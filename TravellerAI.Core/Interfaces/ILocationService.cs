using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface ILocationService
{
    Task<IEnumerable<LocationModel>> GetLocationByCountryAndCity(string country, string? city);
}