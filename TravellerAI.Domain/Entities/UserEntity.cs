namespace TravellerAI.Domain.Entities;

public class UserEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }

    public virtual UserInfoEntity? UserInfo { get; set; }
    public virtual ICollection<JourneyEntity> Journeys { get; set; } = new List<JourneyEntity>();
    public virtual ICollection<TripEntity> Trips { get; set; } = new List<TripEntity>();
    public virtual ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    public virtual ICollection<PlaceEntity> Places { get; set; } = new List<PlaceEntity>();
    public virtual ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
}
