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

    public async Task<IReadOnlyList<TripEntity>> SearchCatalogAsync(Guid countryId, string? city, Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(t => t.Stops)
            .Where(t => t.CountryId == countryId && (t.IsPublic || t.UserId == userId) && (city == null || t.City == city))
            .ToListAsync(cancellationToken);
    }

    public Task<bool> IsUsedAsync(Guid tripId, CancellationToken cancellationToken = default)
    {
        return Context.JourneyDays.AnyAsync(d => d.TripId == tripId, cancellationToken);
    }

    public Task<bool> IsUsedByOthersOrApprovedAsync(Guid tripId, Guid authorId, CancellationToken cancellationToken = default)
    {
        return Context.JourneyDays.AnyAsync(d => d.TripId == tripId && (d.Journey.UserId != authorId || d.Journey.Approved),
            cancellationToken);
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
