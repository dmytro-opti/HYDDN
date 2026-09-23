using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class BookingViewModel
{
    public Guid BookingId { get; set; }
    public Guid UserId { get; set; }
    public Guid? JourneyId { get; set; }
    public Guid? PropertyId { get; set; }
    public Guid? RoomId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; }
    public bool IsPaid { get; set; }
    public string PaymentMethod { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public BookingStatus Status { get; set; }
    /// <summary>Selected booking which cannot be changed anymore.</summary>
    public bool IsFrozen { get; set; }
    public DateTime CreatedAt { get; set; }
}
