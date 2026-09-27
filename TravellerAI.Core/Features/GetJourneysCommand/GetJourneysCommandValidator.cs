using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetJourneysCommand;

public class GetJourneysCommandValidator
{
    public class GetJourneysCommand : IRequest<List<JourneyModel>>
    {
    }
}