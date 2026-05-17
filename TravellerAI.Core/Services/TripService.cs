using TravellerAI.Core.Features.BuildTripCommand;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Services;

public class TripService : ITripService
{
    public Task<Guid> CreateTrip(BuildTripCommand command)
    {
        throw new NotImplementedException();
    }

    Task<TripModel> ITripService.GetTrip(Guid tripId)
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

    public Task SelectPeriod(TripModel trip, PeriodViewModel period)
    {
        throw new NotImplementedException();
    }

    public Task Build(TripModel trip)
    {
        throw new NotImplementedException();
    }

    public Task<TripModel> Show(TripModel trip)
    {
        throw new NotImplementedException();
    }
}