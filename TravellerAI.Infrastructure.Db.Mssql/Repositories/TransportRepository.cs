using Microsoft.EntityFrameworkCore;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Infrastructure.Db.Mssql.Context;

namespace TravellerAI.Infrastructure.Db.Mssql.Repositories;

public class TransportRepository : Repository<TransportEntity>, ITransportRepository
{
    public TransportRepository(TravellerDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<TransportEntity>> GetByJourneyAsync(Guid journeyId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(t => t.JourneyId == journeyId).ToListAsync(cancellationToken);
    }
}
