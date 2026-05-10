using TravellerAI.Core.Features;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Services;

public class TripService : ITripService
{
    public Task<int> CreateTrip(BuildTripCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTrip(int tripId)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteTrip(int tripId)
    {
        throw new NotImplementedException();
    } 
}