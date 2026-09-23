using MediatR;
using TravellerAI.Core.Interfaces;

namespace TravellerAI.Core.Features.User.DeleteAccountCommand;

public class DeleteAccountCommandHandler : IRequestHandler<DeleteAccountCommand, Unit>
{
    private readonly IAuthService _authService;

    public DeleteAccountCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Unit> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        await _authService.DeleteAccountAsync(command.UserId, command.Password);

        return Unit.Value;
    }
}
