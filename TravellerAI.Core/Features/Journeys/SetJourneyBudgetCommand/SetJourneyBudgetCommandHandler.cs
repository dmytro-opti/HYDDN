using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.SetJourneyBudgetCommand;

public class SetJourneyBudgetCommandHandler : IRequestHandler<SetJourneyBudgetCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public SetJourneyBudgetCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(SetJourneyBudgetCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.SetBudgetAsync(command.UserId, command.JourneyId, command.Budget);
    }
}
