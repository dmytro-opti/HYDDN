using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

public class JourneyModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public JourneyStatus Status { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PeriodModel Period { get; set; }
    public List<string> Members { get; set; } = new();
    public bool Approved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public Guid? CountryId { get; set; }
    public string Country { get; set; }
    public BudgetModel Budget { get; set; }
    public List<JourneyDayModel> Days { get; set; } = new();
    /// <summary>Hotel stays (bookings which are not cancelled), ordered by check-in.</summary>
    public List<BookingModel> Hotels { get; set; } = new();
    public List<TransportModel> Transports { get; set; } = new();
}
