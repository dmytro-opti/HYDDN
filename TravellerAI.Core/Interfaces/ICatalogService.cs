using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

/// <summary>
/// Reference data used to set up journeys: countries and activities.
/// </summary>
public interface ICatalogService
{
    Task<IReadOnlyList<CountryModel>> GetCountriesAsync();
    Task<IReadOnlyList<ActivityModel>> GetActivitiesAsync(Guid countryId, string? city);
}
