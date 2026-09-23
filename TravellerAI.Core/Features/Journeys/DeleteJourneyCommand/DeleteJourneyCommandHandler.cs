using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.Journeys.DeleteJourneyCommand;

public class DeleteJourneyCommandHandler : IRequestHandler<DeleteJourneyCommand, Unit>
{
    private readonly IJourneyService _journeyService;

    public DeleteJourneyCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<Unit> Handle(DeleteJourneyCommand command, CancellationToken cancellationToken)
    {
        await _journeyService.DeleteJourneyAsync(command.UserId, command.JourneyId);

        return Unit.Value;
    }
}
