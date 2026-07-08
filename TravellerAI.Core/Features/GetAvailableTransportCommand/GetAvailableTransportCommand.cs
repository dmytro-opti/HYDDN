using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetAvailableTransportCommand;

public class GetAvailableTransportCommand : IRequest<List<TransportModel>>
{
}