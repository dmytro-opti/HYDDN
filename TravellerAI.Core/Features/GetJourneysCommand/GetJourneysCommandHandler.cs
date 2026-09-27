using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetJourneysCommand;

public class GetJourneysCommandHandler : IRequestHandler<GetJourneysCommand, List<JourneyModel>>
{
    private readonly IJourneyService _journeyService;

    public GetJourneysCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<List<JourneyModel>> Handle(
        GetJourneysCommand request,
        CancellationToken cancellationToken)
    {
        return await _journeyService.GetJourneys(request);
    }
}