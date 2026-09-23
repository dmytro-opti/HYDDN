using MediatR;

namespace TravellerAI.Core.Features.Auth.LogoutCommand;

/// <summary>
/// Revokes all refresh tokens of the user (access tokens expire on their own).
/// </summary>
public class LogoutCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
}
