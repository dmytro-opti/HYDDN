using MediatR;

namespace TravellerAI.Core.Features.User.DeleteAccountCommand;

/// <summary>
/// Deletes the account and all user data after the password confirmation.
/// </summary>
public class DeleteAccountCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string Password { get; set; }
}
