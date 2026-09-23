using TravellerAI.Domain.Entities;

namespace TravellerAI.Core.Repositories;

public interface IJourneyRepository : IRepository<JourneyEntity>
{
    public Task<IReadOnlyList<JourneyEntity>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}
