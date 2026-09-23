using TravellerAI.Domain.Entities.Owned;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Entities;

public class BookingEntity : BaseEntity
{
    public Guid UserId { get; set; }
    public virtual UserEntity User { get; set; } = null!;

    public Guid? JourneyId { get; set; }
    public virtual JourneyEntity? Journey { get; set; }

    /// <summary>Booked place (property).</summary>
    public Guid? PropertyId { get; set; }
    public virtual PlaceEntity? Property { get; set; }

    /// <summary>Room of the booked property (rooms are not modelled yet).</summary>
    public Guid? RoomId { get; set; }
    public virtual Period? Period { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; } = "USD";
    public bool IsPaid { get; set; }
    public string? PaymentMethod { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public BookingStatus Status { get; set; }
    public bool IsFrozen { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public Guid? BudgetId { get; set; }
    public virtual BudgetEntity? Budget { get; set; }
}
