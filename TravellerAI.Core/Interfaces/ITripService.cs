using TravellerAI.Core.Features;

namespace TravellerAI.Core.Interfaces;

public interface ITripService
{
    Task<int> CreateTrip(BuildTripCommand command);
    Task<int> GetTrip(int tripId);
    Task<int> DeleteTrip(int tripId);
}