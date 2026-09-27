using System.Diagnostics;
using TravellerAI.Core.Features.GetActivitiesCommand;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface IActivityService
{
    Task<IEnumerable<ActivityModel>> GetActivities(GetActivitiesCommand request);
    
}