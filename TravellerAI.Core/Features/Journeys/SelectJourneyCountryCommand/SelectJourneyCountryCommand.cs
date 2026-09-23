using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Journeys.SelectJourneyCountryCommand;

/// <summary>
/// Step 3: journey country.
/// </summary>
public class SelectJourneyCountryCommand : IRequest<JourneyModel>
{
    public Guid UserId { get; set; }
    public Guid JourneyId { get; set; }
    public Guid CountryId { get; set; }
}
