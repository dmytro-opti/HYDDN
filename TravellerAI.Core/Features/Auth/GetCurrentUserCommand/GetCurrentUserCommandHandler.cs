using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Auth.GetCurrentUserCommand;

public class GetCurrentUserCommandHandler : IRequestHandler<GetCurrentUserCommand, AuthUserModel>
{
    private readonly IAuthService _authService;

    public GetCurrentUserCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthUserModel> Handle(GetCurrentUserCommand command, CancellationToken cancellationToken)
    {
        return _authService.GetCurrentUserAsync(command.UserId);
    }
}
