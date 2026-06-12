using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;

namespace TravellerAI.Core.Features.User.UpdateUserEmailCommand;

public class UpdateUserEmailCommandHandler : IRequestHandler<UpdateUserEmailCommand, Unit>
{
    private readonly IUserService _userService;
    private readonly ILoggerService<UpdateUserEmailCommandHandler> _logger;
    
    
    public UpdateUserEmailCommandHandler(IUserService userService, ILoggerService<UpdateUserEmailCommandHandler> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    public async Task<Unit> Handle(UpdateUserEmailCommand command, CancellationToken cancellationToken)
    {
        await _userService.UpdateEmailAsync(command.UserId, command.Email);
        _logger.Log(ErrorLevel.Low, $"User {command.UserId} email was updated to {command.Email}");

        return Unit.Value;
    }
}