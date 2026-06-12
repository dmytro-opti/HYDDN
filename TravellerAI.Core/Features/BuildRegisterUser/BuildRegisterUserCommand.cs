using MediatR;
using TravellerAI.Domain.Models;


namespace TravellerAI.Core.Features.BuildRegisterUser;

public class BuildRegisterUserCommand : IRequest<UserModel>
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
