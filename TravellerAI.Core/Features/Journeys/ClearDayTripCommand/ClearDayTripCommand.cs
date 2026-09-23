using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.ClearDayTripCommand;

public class ClearDayTripCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public DateTime Date { get; set; }
}
