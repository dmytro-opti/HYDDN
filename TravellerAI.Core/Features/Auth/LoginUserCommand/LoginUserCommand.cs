using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Auth.LoginUserCommand;

public class LoginUserCommand : IRequest<AuthResultModel>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
