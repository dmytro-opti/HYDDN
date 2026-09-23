using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Auth.GetCurrentUserCommand;

public class GetCurrentUserCommand : IRequest<AuthUserModel>
{
    public Guid UserId { get; set; }
}
