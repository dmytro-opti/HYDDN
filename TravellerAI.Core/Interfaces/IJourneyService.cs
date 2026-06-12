using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Interfaces;

public interface IJourneyService
{
    Task<Guid> CreateJourney(JourneyModel journey);
    Task<JourneyModel> GetJourney(Guid JourneyId);
    Task<Guid> DeleteJourney(Guid JourneyId);
    Task SelectPeriod(JourneyModel journey, PeriodViewModel period);
    Task SetMembers(JourneyModel journey, IEnumerable<string> members);
    Task AddTransport(JourneyModel journey, TransportViewModel transport);
}