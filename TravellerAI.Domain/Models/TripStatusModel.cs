using TravellerAI.Domain.Enums;

namespace TravellerAI.Domain.Models;

public class TripStatusModel
{
    public Guid TripId { get; set; }
    public TripStatus Status { get; set; }
}