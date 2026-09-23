using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Auth.LoginUserCommand;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResultModel>
{
    private readonly IAuthService _authService;

    public LoginUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResultModel> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        return _authService.LoginAsync(new LoginUserModel { Email = command.Email, Password = command.Password });
    }
}
