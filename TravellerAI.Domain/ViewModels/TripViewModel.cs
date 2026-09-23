using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class TripViewModel
{
    public Guid TripId { get; set; }
    public string Name { get; set; }
    public TripStatus Status { get; set; }
    public double Rating { get; set; }
    public PeriodViewModel Period { get; set; }
    public Guid UserId { get; set; }
    public Guid? JourneyId { get; set; }
    public string JourneyTitle { get; set; }
    public BudgetViewModel Budget { get; set; }
    public BookingViewModel Booking { get; set; }
    public List<TransportViewModel> Transports { get; set; }
}
