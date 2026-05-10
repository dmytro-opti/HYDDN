using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features;

public class BuildTripCommand : IRequest<int>
{
    public string Name  { get; set; }
    public UserModel User { get; set; }
    public GroupModel Group { get; set; }
    public PeriodModel Period { get; set; }
}