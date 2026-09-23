using Microsoft.EntityFrameworkCore;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Infrastructure.Db.Mssql.Context;

namespace TravellerAI.Infrastructure.Db.Mssql.Repositories;

public class TripRepository : Repository<TripEntity>, ITripRepository
{
    public TripRepository(TravellerDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<TripEntity>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(t => t.UserId == userId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TripEntity>> GetByJourneyAsync(Guid journeyId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(t => t.JourneyId == journeyId).ToListAsync(cancellationToken);
    }
}
