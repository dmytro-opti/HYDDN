using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.SetJourneyMembersCommand;

public class SetJourneyMembersCommandHandler : IRequestHandler<SetJourneyMembersCommand, JourneyModel>
{
    private readonly IJourneyService _journeyService;

    public SetJourneyMembersCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyModel> Handle(SetJourneyMembersCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.SetMembersAsync(command.UserId, command.JourneyId, command.Members);
    }
}
