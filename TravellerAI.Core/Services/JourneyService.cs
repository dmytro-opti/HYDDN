using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Services;

public class JourneyService : IJourneyService
{
    public Task<Guid> CreateJourney(JourneyModel journey)
    {
        return Task.FromResult(Guid.NewGuid());
    }

    Task<JourneyModel> IJourneyService.GetJourney(Guid tripId)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> GetJourney(Guid tripId)
    {
        return Task.FromResult(tripId);
    }

    public Task<Guid> DeleteJourney(Guid tripId)
    {
        return Task.FromResult(tripId);
    }

    public Task SelectPeriod(JourneyModel journey, PeriodViewModel period)
    {
        throw new NotImplementedException();
    }

    public Task SetMembers(JourneyModel journey, IEnumerable<string> members)
    {
        throw new NotImplementedException();
    }

    public Task AddTransport(JourneyModel journey, TransportViewModel transport)
    {
        throw new NotImplementedException();
    }
}