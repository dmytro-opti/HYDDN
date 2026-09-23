using Microsoft.EntityFrameworkCore;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Infrastructure.Db.Mssql.Context;

namespace TravellerAI.Infrastructure.Db.Mssql.Repositories;

public class JourneyRepository : Repository<JourneyEntity>, IJourneyRepository
{
    public JourneyRepository(TravellerDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<JourneyEntity>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(j => j.UserId == userId).ToListAsync(cancellationToken);
    }

    public override async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // bookings (hotel history) use ClientSetNull, so they are loaded to be detached from the journey;
        // days and transports are removed by database cascade
        var journey = await DbSet
            .Include(j => j.Bookings)
            .FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

        if (journey == null)
        {
            return false;
        }

        DbSet.Remove(journey);
        await SaveChangesAsync(cancellationToken);

        return true;
    }
}
