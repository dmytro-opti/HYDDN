using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.Journeys.CreateJourneyCommand;

public class CreateJourneyCommandHandler : IRequestHandler<CreateJourneyCommand, Guid>
{
    private readonly IJourneyService _journeyService;

    public CreateJourneyCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<Guid> Handle(CreateJourneyCommand command, CancellationToken cancellationToken)
    {
        return await _journeyService.CreateJourneyAsync(command.UserId, command.Title, command.Description, command.Members);
    }
}
