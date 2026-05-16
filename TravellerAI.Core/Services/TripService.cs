using TravellerAI.Core.Features.BuildTripCommand;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Services;

public class TripService : ITripService
{
    public Task<Guid> CreateTrip(BuildTripCommand command)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> GetTrip(Guid tripId)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> DeleteTrip(Guid tripId)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> AddPeriodTrip(BuildTripCommand command)
    {
        throw new NotImplementedException();
    }
}