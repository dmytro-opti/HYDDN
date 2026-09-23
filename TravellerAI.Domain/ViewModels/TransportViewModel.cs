using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels;

public class TransportViewModel
{
    public Guid Id { get; set; }
    public Guid JourneyId { get; set; }
    public TransportType Type { get; set; }
    public string Company { get; set; }
    public decimal Price { get; set; }
    public PeriodViewModel Period { get; set; }
    public TimeSpan Duration { get; set; }
    public SeatClass SeatClass { get; set; }
    public int SeatCount { get; set; }
}
