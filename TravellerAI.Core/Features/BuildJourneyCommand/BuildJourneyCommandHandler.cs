using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Exceptions;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.BuildJourneyCommand;

public class BuildJourneyCommandHandler : IRequestHandler<BuildJourneyCommand, int>
{
    private readonly IJourneyService _journeyService;
    private readonly IUserService _userService;
    
    public BuildJourneyCommandHandler(IJourneyService journeyService, IUserService userService)
    {
        _journeyService = journeyService;
        _userService = userService;
    }
    public async Task<int> Handle(BuildJourneyCommand command, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(command.UserId);

        if (user == null)
        {
            throw new ResourceNotFoundException("User not found");
        }
        
        return await _journeyService.CreateJourney(command);
    }
}

public interface IUserService
{
    public Task<UserModel> GetUserAsync(Guid userId);
}