using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.ViewModels;

namespace TravellerAI.Core.Features.CreateJourneyCommand;

public class CreateJourneyCommandHandler 
    : IRequestHandler<CreateJourneyCommand, JourneyViewModel>
{
    private readonly IJourneyService _journeyService;

    public CreateJourneyCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<JourneyViewModel> Handle(
        CreateJourneyCommand request,
        CancellationToken cancellationToken)
    {
        return await _journeyService.CreateJourney(request.Journey);
    }
}