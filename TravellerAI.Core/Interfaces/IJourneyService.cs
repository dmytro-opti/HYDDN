using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Interfaces;

public interface IJourneyService
{
    Task<Guid> CreateJourney(BuildJourneyCommand command);
    Task<JourneyModel> GetJourneyAsync(Guid journeyId);
    Task<Guid> DeleteJourney(Guid tripId);
    Task SelectPeriod(JourneyModel journey, PeriodViewModel period);
    Task SetMembers(JourneyModel journey, IEnumerable<string> members);
    Task<JourneyStatus> GetJourneyStatusAsync(Guid journeyId);
}