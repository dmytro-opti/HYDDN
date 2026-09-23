using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Auth.RegisterUserCommand;

public class RegisterUserCommand : IRequest<AuthResultModel>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
