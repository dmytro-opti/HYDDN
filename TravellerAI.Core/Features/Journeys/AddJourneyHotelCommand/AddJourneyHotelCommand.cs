using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.AddJourneyHotelCommand;

/// <summary>
/// Step 4: hotel stay. Several stays mean hotel switches; together they cover every night.
/// </summary>
public class AddJourneyHotelCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public Guid PlaceId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
}
