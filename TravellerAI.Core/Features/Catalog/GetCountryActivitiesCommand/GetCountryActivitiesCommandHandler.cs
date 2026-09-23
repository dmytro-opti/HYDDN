using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Catalog.GetCountryActivitiesCommand;

public class GetCountryActivitiesCommandHandler : IRequestHandler<GetCountryActivitiesCommand, List<ActivityModel>>
{
    private readonly ICatalogService _catalogService;

    public GetCountryActivitiesCommandHandler(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task<List<ActivityModel>> Handle(GetCountryActivitiesCommand command, CancellationToken cancellationToken)
    {
        return (await _catalogService.GetActivitiesAsync(command.CountryId, command.City)).ToList();
    }
}
