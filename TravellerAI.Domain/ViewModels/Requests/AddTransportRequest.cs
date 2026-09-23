using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels.Requests;

public class AddTransportRequest
{
    public Guid? JourneyId { get; set; }
    public TransportType Type { get; set; }
    public string Company { get; set; }
    public SeatClass SeatClass { get; set; }
    public int SeatCount { get; set; }
    public decimal Price { get; set; }
}
