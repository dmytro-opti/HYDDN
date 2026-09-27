using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetActivitiesCommand;

public class GetActivitiesCommandHandler 
    : IRequestHandler<GetActivitiesCommand, List<ActivityModel>>
{
    private readonly IActivityService _activityService;

    public GetActivitiesCommandHandler(IActivityService activityService)
    {
        _activityService = activityService;
    }

    public async Task<List<ActivityModel>> Handle(
        GetActivitiesCommand request,
        CancellationToken cancellationToken)
    {
        var activities = await _activityService.GetActivities(request);
        return activities.ToList();
    }
}