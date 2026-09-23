namespace TravellerAI.Domain.Entities;

/// <summary>
/// User profile. Credentials (password hash, roles, lockout) are stored by ASP.NET Core Identity
/// in a separate identity user with the same Id.
/// </summary>
public class UserEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    /// <summary>Copy of the identity email, kept in sync by the identity service.</summary>
    public string Email { get; set; } = string.Empty;

    public virtual UserInfoEntity? UserInfo { get; set; }
    public virtual ICollection<JourneyEntity> Journeys { get; set; } = new List<JourneyEntity>();
    public virtual ICollection<TripEntity> Trips { get; set; } = new List<TripEntity>();
    public virtual ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    public virtual ICollection<PlaceEntity> Places { get; set; } = new List<PlaceEntity>();
    public virtual ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
    public virtual ICollection<NotificationEntity> Notifications { get; set; } = new List<NotificationEntity>();
}
