using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Entities;

public class ActivityEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ActivityType Type { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public JourneyStatus Status { get; set; }
    public decimal Rating { get; set; }
    public string? ImageUrl { get; set; }
    public string? VideoUrl { get; set; }
    public virtual Period? Period { get; set; }

    public Guid? LocationId { get; set; }
    public virtual LocationEntity? Location { get; set; }

    public virtual ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
}
