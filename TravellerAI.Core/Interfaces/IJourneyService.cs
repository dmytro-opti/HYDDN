using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Features.GetJourneysCommand;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Interfaces;

public interface IJourneyService
{
    Task<JourneyViewModel> CreateJourney(JourneyViewModel journey);
    Task<Guid> CreateJourney(BuildJourneyCommand command);
    Task<JourneyModel> GetJourneyAsync(Guid tripId);
    Task<Guid> DeleteJourney(Guid tripId);
    Task SelectPeriod(JourneyModel journey, PeriodViewModel period);
    Task SetMembers(JourneyModel journey, IEnumerable<string> members);
    Task AddTransport(JourneyModel journey, TransportViewModel transport);
    Task<List<JourneyModel>> GetJourneys(GetJourneysCommand request);
}