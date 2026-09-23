using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Entities;

public class PlaceEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Type { get; set; }
    public string? Address { get; set; }
    public decimal PricePerHour { get; set; }
    public int Capacity { get; set; }
    public bool IsAvailable { get; set; }
    public PlaceStatus Status { get; set; }
    public double AverageRating { get; set; }
    public List<string> ImageUrls { get; set; } = new();
    public FoodOptions Food { get; set; }
    public string? BookingRules { get; set; }

    public Guid? LocationId { get; set; }
    public virtual LocationEntity? Location { get; set; }

    public Guid? OwnerId { get; set; }
    public virtual UserEntity? Owner { get; set; }

    public virtual ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
    public virtual ICollection<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
}
