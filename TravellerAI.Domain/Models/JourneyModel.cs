using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

public class JourneyModel
{
    public Guid JourneyId { get; set; }
    public Guid UserId { get; set; }
    public JourneyStatus Status { get; set; }
    public PeriodModel Period { get; set; }
    public BudgetModel Budget { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IEnumerable<UserInfoModel> Members { get; set; }
    public IEnumerable<ActivityModel> Activities { get; set; }
    public IEnumerable<BookingModel> Bookings { get; set; }
    public UserModel User { get; set; }
    public TransportModel Transport { get; set; }
}

