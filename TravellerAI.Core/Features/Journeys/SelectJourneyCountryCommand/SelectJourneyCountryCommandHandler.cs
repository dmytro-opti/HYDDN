using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.SelectJourneyCountryCommand;

public class SelectJourneyCountryCommandHandler : IRequestHandler<SelectJourneyCountryCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public SelectJourneyCountryCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(SelectJourneyCountryCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.SelectCountryAsync(command.UserId, command.JourneyId, command.CountryId);
    }
}
