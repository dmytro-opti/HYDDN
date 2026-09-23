using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.ViewModels.Requests;

public class TransportSearchRequest
{
    public TransportType? Type { get; set; }
    public SeatClass? SeatClass { get; set; }
    public string? Company { get; set; }
    public decimal? MaxPrice { get; set; }
    public int MinSeats { get; set; } = 1;
    public DateTime? DepartureFrom { get; set; }
    public DateTime? DepartureTo { get; set; }
}
