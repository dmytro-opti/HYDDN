using TravellerAI.Domain.Entities;

namespace TravellerAI.Core.Repositories;

public interface ITransportRepository : IRepository<TransportEntity>
{
    public Task<IReadOnlyList<TransportEntity>> GetByJourneyAsync(Guid journeyId, CancellationToken cancellationToken = default);
}
