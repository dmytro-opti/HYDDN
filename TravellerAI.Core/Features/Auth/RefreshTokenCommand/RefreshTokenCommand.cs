using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Auth.RefreshTokenCommand;

public class RefreshTokenCommand : IRequest<AuthResultModel>
{
    public string RefreshToken { get; set; }
}
