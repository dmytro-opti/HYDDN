using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.AssignDayTripCommand;

/// <summary>
/// Step 5: trip of the day.
/// </summary>
public class AssignDayTripCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public DateTime Date { get; set; }
    public Guid TripId { get; set; }
}
