using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Auth.RefreshTokenCommand;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResultModel>
{
    private readonly IAuthService _authService;

    public RefreshTokenCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public Task<AuthResultModel> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        return _authService.RefreshAsync(command.RefreshToken);
    }
}
