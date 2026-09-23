using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.GetDayTripsCommand;

/// <summary>
/// Trips which fit the day: from the hotel of the previous night to the hotel of the coming night.
/// </summary>
public class GetDayTripsCommand : IRequest<List<TripModel>>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public DateTime Date { get; set; }
}
