using TravellerAI.Core.Features.BuildJourneyCommand;

namespace TravellerAI.Core.Interfaces;

public interface IJourneyService
{
    Task<int> CreateJourney(BuildJourneyCommand command);
    Task<int> GetJourney(int tripId);
    Task<int> DeleteJourney(int tripId);
}