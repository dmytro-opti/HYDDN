using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildJourneyCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public PeriodModel Period { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IEnumerable<Guid> MemberIds { get; set; }
    public BookingDto Booking { get; set; }
    public TransportDto Transport { get; set; }
    public Guid JourneyId { get; set; }
    
}

public class BookingDto
{
    public Guid PropertyId { get; set; }
    public Guid RoomId { get; set; }
    public int GuestCount { get; set; }
}

public class TransportDto
{
    public TransportType Type { get; set; }
    public string Company { get; set; }
    public int SeatCount { get; set; }
    public SeatClass SeatClass { get; set; }
}