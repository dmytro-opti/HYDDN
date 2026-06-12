using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;


namespace TravellerAI.Core.Features.BuildRegisterUser;

public class BuildRegisterUserCommandHandler : IRequestHandler<BuildRegisterUserCommand, UserModel>
{
    private readonly IUserService _userService;

    public BuildRegisterUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<UserModel> Handle(BuildRegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = new UserModel()
        {
            Id = command.UserId,
            Name = command.Name,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Password = command.Password,
            Email = command.Email
        };
        return await _userService.CreateUser(user);
    }
}