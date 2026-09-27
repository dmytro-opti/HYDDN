using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Features.GetJourneysCommand;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Services;

public class JourneyService : IJourneyService
{
    public Task<JourneyViewModel> CreateJourney(JourneyViewModel journey)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreateJourney(BuildJourneyCommand command)
    {
        return Task.FromResult(Guid.NewGuid());
    }

    Task<JourneyModel> IJourneyService.GetJourneyAsync(Guid tripId)
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

    public Task<List<JourneyModel>> GetJourneys(GetJourneysCommand request)
    {
        throw new NotImplementedException();
    }

    public Task<List<JourneyModel>> GetJourneys(GetJourneysCommand request)
    {
        throw new NotImplementedException();
    }
}