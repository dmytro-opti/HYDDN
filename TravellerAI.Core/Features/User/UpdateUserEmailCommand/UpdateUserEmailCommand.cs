using MediatR;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.User.UpdateUserEmailCommand;

public class UpdateUserEmailCommand : IRequest<Unit>
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
}