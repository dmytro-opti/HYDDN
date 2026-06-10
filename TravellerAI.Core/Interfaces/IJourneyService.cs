using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Interfaces;

public interface IJourneyService
{
    Task<Guid> CreateJourney(BuildJourneyCommand command);
    Task<JourneyModel> GetJourney(Guid tripId);
    Task<Guid> DeleteJourney(Guid tripId);
    Task SelectPeriod(JourneyModel journey, PeriodViewModel period);
    Task SetMembers(JourneyModel journey, IEnumerable<string> members);
    Task AddTransport(JourneyModel journey, TransportViewModel transport);
}