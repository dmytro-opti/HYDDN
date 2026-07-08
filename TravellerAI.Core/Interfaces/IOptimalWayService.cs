using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface IOptimalWayService
{
     Task<OptimalWayModel> BuildOptimalWayAsync(OptimalWayModel optimalWay);
}