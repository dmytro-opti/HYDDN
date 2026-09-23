namespace TravellerAI.Domain.Entities.Owned;

/// <summary>
/// Value object stored as columns of the owning table (EF Core owned type),
/// therefore it has no identity and does not inherit BaseEntity.
/// </summary>
public class Period
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
