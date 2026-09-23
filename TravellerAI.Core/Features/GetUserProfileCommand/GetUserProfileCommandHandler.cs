using MediatR;
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

    public Task<UserInfoModel> Handle(GetUserProfileCommand command, CancellationToken cancellationToken)
    {
        // throws NotFoundException when the user or the profile info does not exist
        return _userService.GetUserInfoAsync(command.UserId);
    }
}
