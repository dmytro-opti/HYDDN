using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

public class TransportModel
{
    public TransportType Type { get; set; }
    public string Company { get; set; }
    public decimal Price { get; set; }
    public PeriodModel Period { get; set; }
    public SeatClass SeatClass { get; set; }
    public TimeSpan Duration { get; set; }
    public bool IsAvailable { get; set; }
}