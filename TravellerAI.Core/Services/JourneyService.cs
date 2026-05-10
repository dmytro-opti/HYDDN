using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Services;

public class JourneyService : IJourneyService
{
    public Task<int> CreateJourney(BuildJourneyCommand command)
    {
        return Task.FromResult(0);
    }

    public Task<int> GetJourney(int tripId)
    {
        return Task.FromResult(tripId);
    }

    public Task<int> DeleteJourney(int tripId)
    {
        return Task.FromResult(tripId);
    }
}