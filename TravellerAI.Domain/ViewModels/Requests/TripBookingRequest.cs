namespace TravellerAI.Domain.ViewModels.Requests;

public class TripBookingRequest
{
    /// <summary>Id of the current trip booking to update; empty to create a new booking.</summary>
    public Guid? BookingId { get; set; }
    public Guid? PropertyId { get; set; }
    public Guid? RoomId { get; set; }
    public PeriodViewModel Period { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public decimal TotalPrice { get; set; }
    public string? Currency { get; set; }
    public string? PaymentMethod { get; set; }
}
