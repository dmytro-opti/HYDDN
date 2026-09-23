using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Entities;

public class JourneyEntity : BaseEntity
{
    public JourneyStatus Status { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public virtual Period? Period { get; set; }

    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public virtual ICollection<TripEntity> Trips { get; set; } = new List<TripEntity>();
    public virtual ICollection<TransportEntity> Transports { get; set; } = new List<TransportEntity>();
    public virtual ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
}
