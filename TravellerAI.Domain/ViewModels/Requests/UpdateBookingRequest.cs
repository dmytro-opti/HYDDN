namespace TravellerAI.Domain.ViewModels.Requests;

public class UpdateBookingRequest
{
    public Guid UserId { get; set; }
    public Guid PropertyId { get; set; }
    public Guid RoomId { get; set; }
    public string? PaymentMethod { get; set; }
    public PeriodViewModel Period { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
}
