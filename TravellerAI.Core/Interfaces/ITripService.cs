using TravellerAI.Core.Features.BuildTripCommand;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Interfaces;

public interface ITripService
{
    Task<Guid> CreateTrip(BuildTripCommand command);
    Task<TripModel> GetTrip(Guid tripId);
    Task<Guid> DeleteTrip(Guid tripId);
    Task<Guid> AddPeriodTrip(BuildTripCommand command);
    Task SelectPeriod(TripModel trip, PeriodViewModel period);
    Task Build(TripModel trip);
    Task<TripModel> Show(TripModel trip);
}