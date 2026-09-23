using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.RemoveJourneyHotelCommand;

public class RemoveJourneyHotelCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public Guid BookingId { get; set; }
}
