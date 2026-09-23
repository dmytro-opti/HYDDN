using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.Auth.ChangePasswordCommand;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
{
    private readonly IAuthService _authService;

    public ChangePasswordCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Unit> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
    {
        await _authService.ChangePasswordAsync(command.UserId, command.CurrentPassword, command.NewPassword);

        return Unit.Value;
    }
}
