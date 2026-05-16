using TravellerAI.Core.Features.BuildJourneyCommand;

namespace TravellerAI.Core.Interfaces;

public interface ITripService
{
    Task<Guid> CreateTrip(BuildTripCommand command);
    Task<Guid> GetTrip(Guid tripId);
    Task<Guid> DeleteTrip(Guid tripId);
}