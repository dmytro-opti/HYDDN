using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Entities;

public class TripEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public double Rating { get; set; }
    public TripStatus Status { get; set; }
    public virtual Period? Period { get; set; }

    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public Guid? JourneyId { get; set; }
    public virtual JourneyEntity? Journey { get; set; }

    public Guid? BudgetId { get; set; }
    public virtual BudgetEntity? Budget { get; set; }

    public Guid? BookingId { get; set; }
    public virtual BookingEntity? Booking { get; set; }

    public virtual ICollection<TransportEntity> Transports { get; set; } = new List<TransportEntity>();
}
