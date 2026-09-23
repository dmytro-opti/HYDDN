using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Entities;

public class TransportEntity : BaseEntity
{
    public Guid TripId { get; set; }
    public virtual TripEntity Trip { get; set; } = null!;

    public Guid? JourneyId { get; set; }
    public virtual JourneyEntity? Journey { get; set; }

    public TransportType Type { get; set; }
    public string Company { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public virtual Period? Period { get; set; }
    public SeatClass SeatClass { get; set; }
    public int SeatCount { get; set; }
    public TimeSpan Duration { get; set; }
}
