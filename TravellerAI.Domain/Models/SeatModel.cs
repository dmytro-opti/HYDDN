using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

public class SeatModel
{
    public Guid SeatId { get; set; }
    public Guid TransportId { get; set; }
    public string SeatNumber { get; set; } 
    public SeatClass SeatClass { get; set; }
    public bool IsAvailable { get; set; }
}