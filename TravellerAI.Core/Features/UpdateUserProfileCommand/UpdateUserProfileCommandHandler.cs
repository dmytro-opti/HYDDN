using AutoMapper;
using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.UpdateUserProfileCommand;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserModel>
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UpdateUserProfileCommandHandler(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<UserModel> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        // throws NotFoundException when missing
        var user = await _userService.GetUserAsync(request.UserId);

        _mapper.Map(request, user);
        await _userService.UpdateUserProfileAsync(user);

        var updated = await _userService.GetUserAsync(request.UserId);
        // never return credentials to the caller
        updated.Password = string.Empty;

        return updated;
    }
}
