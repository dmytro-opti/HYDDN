using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.GetMyJourneysCommand;

public class GetMyJourneysCommandHandler : IRequestHandler<GetMyJourneysCommand, List<JourneyModel>>
{
    private readonly IJourneyService _journeyService;

    public GetMyJourneysCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<List<JourneyModel>> Handle(GetMyJourneysCommand command, CancellationToken cancellationToken)
    {
        return (await _journeyService.GetUserJourneysAsync(command.UserId, command.Approved)).ToList();
    }
}
