using TravellerAI.Core.Features.BuildTripCommand;

namespace TravellerAI.Core.Interfaces;

public interface ITripService
{
    Task<Guid> CreateTrip(BuildTripCommand command);
    Task<Guid> GetTrip(Guid tripId);
    Task<Guid> DeleteTrip(Guid tripId);
    Task<Guid> AddPeriodTrip(BuildTripCommand command);
}