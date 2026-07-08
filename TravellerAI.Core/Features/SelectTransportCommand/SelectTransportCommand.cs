using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.SelectTransportCommand;

public class SelectTransportCommand : IRequest<TransportModel>
{
    public Guid TransportId { get; set; }
}