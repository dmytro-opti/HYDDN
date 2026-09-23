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

    public override async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // "chosen trips" links use ClientCascade (a DB cascade would add a second path from Users),
        // so they are loaded to be removed together with the trip
        var trip = await DbSet
            .Include(t => t.ChosenBy)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (trip == null)
        {
            return false;
        }

        DbSet.Remove(trip);
        await SaveChangesAsync(cancellationToken);

        return true;
    }
}
