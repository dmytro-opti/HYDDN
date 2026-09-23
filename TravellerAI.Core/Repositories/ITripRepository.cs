using TravellerAI.Domain.Entities;

namespace TravellerAI.Core.Repositories;

public interface ITripRepository : IRepository<TripEntity>
{
    public Task<IReadOnlyList<TripEntity>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyList<TripEntity>> GetByJourneyAsync(Guid journeyId, CancellationToken cancellationToken = default);
}
