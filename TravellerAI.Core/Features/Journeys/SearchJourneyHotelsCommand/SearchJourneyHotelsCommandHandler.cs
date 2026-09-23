using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.SearchJourneyHotelsCommand;

public class SearchJourneyHotelsCommandHandler : IRequestHandler<SearchJourneyHotelsCommand, List<HotelOfferModel>>
{
    private readonly IJourneyService _journeyService;

    public SearchJourneyHotelsCommandHandler(IJourneyService journeyService)
    {
        _journeyService = journeyService;
    }

    public async Task<List<HotelOfferModel>> Handle(SearchJourneyHotelsCommand command, CancellationToken cancellationToken)
    {
        return (await _journeyService.SearchHotelsAsync(command.UserId, command.JourneyId, command.CheckIn, command.CheckOut, command.City, command.Guests)).ToList();
    }
}
