using MediatR;

namespace TravellerAI.Core.Features.Auth.ChangePasswordCommand;

public class ChangePasswordCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}
