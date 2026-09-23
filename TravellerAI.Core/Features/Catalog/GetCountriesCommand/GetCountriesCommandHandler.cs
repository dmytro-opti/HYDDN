using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Catalog.GetCountriesCommand;

public class GetCountriesCommandHandler : IRequestHandler<GetCountriesCommand, List<CountryModel>>
{
    private readonly ICatalogService _catalogService;

    public GetCountriesCommandHandler(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task<List<CountryModel>> Handle(GetCountriesCommand command, CancellationToken cancellationToken)
    {
        return (await _catalogService.GetCountriesAsync()).ToList();
    }
}
