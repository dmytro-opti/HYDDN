using MediatR;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.GetUserProfileCommand;

public class GetUserProfileCommand : IRequest<UserInfoModel>
{
    public Guid UserId { get; set; }
}