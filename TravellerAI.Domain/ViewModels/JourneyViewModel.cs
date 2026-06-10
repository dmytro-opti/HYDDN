namespace TravellerAI.Domain.ViewModels;

public class JourneyViewModel
{
    public Guid Id { get; set; }
    public IEnumerable<string> Members { get; set; }
    public int Budget { get; set; }
    public PeriodViewModel Period { get; set; }
    public TransportViewModel Transport { get; set; }
    public BookingViewModel Booking { get; set; }
}