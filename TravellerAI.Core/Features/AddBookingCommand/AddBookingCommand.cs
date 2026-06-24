using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.AddBookingCommand;

public class AddBookingCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public Guid TripId { get; set; }
    public PeriodModel Period { get; set; }
    public int Children { get; set; }
    public int Adults { get; set; }
}