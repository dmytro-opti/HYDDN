using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class JourneyViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public JourneyStatus Status { get; set; }
    /// <summary>Setup is finished and the journey is read-only (use unapprove to edit).</summary>
    public bool Approved { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public PeriodViewModel Period { get; set; }
    public Guid? CountryId { get; set; }
    public string Country { get; set; }
    public List<string> Members { get; set; }
    public BudgetViewModel Budget { get; set; }
    public List<JourneyDayViewModel> Days { get; set; }
    public List<BookingViewModel> Hotels { get; set; }
    public List<TransportViewModel> Transports { get; set; }
}
