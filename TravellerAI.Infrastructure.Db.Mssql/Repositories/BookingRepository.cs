using Microsoft.EntityFrameworkCore;
using TravellerAI.Core.Repositories;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Enums;
using TravellerAI.Infrastructure.Db.Mssql.Context;

namespace TravellerAI.Infrastructure.Db.Mssql.Repositories;

public class BookingRepository : Repository<BookingEntity>, IBookingRepository
{
    public BookingRepository(TravellerDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<BookingEntity>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(b => b.UserId == userId).ToListAsync(cancellationToken);
    }

    public Task<bool> HasOverlappingBookingAsync(Guid propertyId, DateTime checkIn, DateTime checkOut,
        Guid? exceptBookingId = null, CancellationToken cancellationToken = default)
    {
        return DbSet.AnyAsync(b =>
                b.PropertyId == propertyId
                && b.Id != exceptBookingId
                && b.Status != BookingStatus.Cancelled
                && b.CheckInDate < checkOut
                && checkIn < b.CheckOutDate,
            cancellationToken);
    }
}
