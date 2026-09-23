using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.Auth.LogoutCommand;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
{
    private readonly IAuthService _authService;

    public LogoutCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Unit> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        await _authService.LogoutAsync(command.UserId);

        return Unit.Value;
    }
}
