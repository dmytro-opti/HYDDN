using TravellerAI.Core.Features.BuildJourneyCommand;

namespace TravellerAI.Core.Interfaces;

public interface IJourneyService
{
    Task<Guid> CreateJourney(BuildJourneyCommand command);
    Task<Guid> GetJourney(Guid tripId);
    Task<Guid> DeleteJourney(Guid tripId);
}