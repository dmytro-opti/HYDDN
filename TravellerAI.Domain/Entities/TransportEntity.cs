using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Entities;

public class TransportEntity : BaseEntity
{
    /// <summary>Transport to the country / between cities of the journey.</summary>
    public Guid JourneyId { get; set; }
    public virtual JourneyEntity Journey { get; set; } = null!;

    public TransportType Type { get; set; }
    public string Company { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public virtual Period? Period { get; set; }
    public SeatClass SeatClass { get; set; }
    public int SeatCount { get; set; }
    public TimeSpan Duration { get; set; }
}
