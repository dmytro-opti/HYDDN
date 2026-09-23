using TravellerAI.Domain.Entities;

namespace TravellerAI.Core.Repositories;

public interface IBookingRepository : IRepository<BookingEntity>
{
    public Task<IReadOnlyList<BookingEntity>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether the property already has a non-cancelled booking intersecting the given dates.
    /// </summary>
    public Task<bool> HasOverlappingBookingAsync(Guid propertyId, DateTime checkIn, DateTime checkOut,
        Guid? exceptBookingId = null, CancellationToken cancellationToken = default);
}
