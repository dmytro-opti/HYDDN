using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Entities;

/// <summary>
/// Journey is set up step by step: period, country, hotel stays, day trips.
/// While <see cref="Approved"/> is false the journey is a draft and can be edited.
/// </summary>
public class JourneyEntity : BaseEntity
{
    public JourneyStatus Status { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public virtual Period? Period { get; set; }
    public List<string> Members { get; set; } = new();

    /// <summary>Setup is finished; all edits are disabled until the journey is unapproved.</summary>
    public bool Approved { get; set; }
    public DateTime? ApprovedAt { get; set; }

    public Guid? CountryId { get; set; }
    public virtual CountryEntity? Country { get; set; }

    public Guid? BudgetId { get; set; }
    public virtual BudgetEntity? Budget { get; set; }

    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public virtual ICollection<JourneyDayEntity> Days { get; set; } = new List<JourneyDayEntity>();
    /// <summary>Hotel stays of the journey (not cancelled bookings), they cover every night of the period.</summary>
    public virtual ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    public virtual ICollection<TransportEntity> Transports { get; set; } = new List<TransportEntity>();
}
