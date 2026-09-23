using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Transports.SearchTransportsCommand;

public class SearchTransportsCommand : IRequest<List<TransportModel>>
{
    public TransportType? Type { get; set; }
    public SeatClass? SeatClass { get; set; }
    public string? Company { get; set; }
    public decimal? MaxPrice { get; set; }
    public int MinSeats { get; set; }
    public DateTime? DepartureFrom { get; set; }
    public DateTime? DepartureTo { get; set; }
}
