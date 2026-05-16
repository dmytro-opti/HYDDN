using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Services;

public class JourneyService : IJourneyService
{
    public Task<Guid> CreateJourney(BuildJourneyCommand command)
    {
        return Task.FromResult(Guid.NewGuid());
    }

    public Task<Guid> GetJourney(Guid tripId)
    {
        return Task.FromResult(tripId);
    }

    public Task<Guid> DeleteJourney(Guid tripId)
    {
        return Task.FromResult(tripId);
    }
}