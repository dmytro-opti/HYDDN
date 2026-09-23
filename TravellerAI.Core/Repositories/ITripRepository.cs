using TravellerAI.Domain.Entities;

namespace TravellerAI.Core.Repositories;

public interface ITripRepository : IRepository<TripEntity>
{
    /// <summary>Public trips and own trips of the country (optionally city).</summary>
    public Task<IReadOnlyList<TripEntity>> SearchCatalogAsync(Guid countryId, string? city, Guid userId, CancellationToken cancellationToken = default);
    /// <summary>The trip is scheduled in any journey.</summary>
    public Task<bool> IsUsedAsync(Guid tripId, CancellationToken cancellationToken = default);
    /// <summary>The trip is scheduled in journeys of other users or in approved journeys.</summary>
    public Task<bool> IsUsedByOthersOrApprovedAsync(Guid tripId, Guid authorId, CancellationToken cancellationToken = default);
}
