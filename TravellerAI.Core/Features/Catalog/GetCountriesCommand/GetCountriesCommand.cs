using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Catalog.GetCountriesCommand;

public class GetCountriesCommand : IRequest<List<CountryModel>>
{

}
