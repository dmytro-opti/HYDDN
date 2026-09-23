namespace TravellerAI.Domain.Entities;

/// <summary>
/// Base class for all persisted entities.
/// Created / Updated are filled automatically by TravellerDbContext on SaveChanges.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
