using MediatR;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Features.BuildJourneyCommand;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetUserProfileCommand;

public class GetUserProfileCommandHandler : IRequestHandler<GetUserProfileCommand, UserInfoModel>
{
    private readonly IUserService _userService;
    public GetUserProfileCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserInfoModel> Handle(GetUserProfileCommand command, CancellationToken cancellationToken)
    {
        var userInfo = await _userService.GetUserInfoAsync(command.UserId);
        if (userInfo == null)
        {
            throw new BadRequestException($"User with ID {command.UserId} not found.");
        }

        return userInfo;
    }
}