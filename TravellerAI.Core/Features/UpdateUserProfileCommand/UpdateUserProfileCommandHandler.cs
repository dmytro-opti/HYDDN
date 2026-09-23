using MediatR;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.UpdateUserProfileCommand;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserModel>
{
    private readonly IUserService _userService;
    public UpdateUserProfileCommandHandler(IUserService userService)
    {
        _userService = userService;
    }
    public async Task<UserModel> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException($"User with ID {request.UserId} not found.");
        }
        
        user.Id = request.UserId;
        user.Name = request.Name;
        user.LastName = request.LastName;
        user.Password = request.Password;
        user.Email = request.Email;
        user.Interests = request.Interests;
        user.TravelStyle = request.TravelStyle;
        user.LookingFor = request.LookingFor;
        user.Languages = request.Languages;
        user.PersonalityType = request.PersonalityType;
        user.ChoosenActivity = request.ChoosenActivity;
        user.ChoosenTrip = request.ChoosenTrip;
        user.MoneyAmount = request.MoneyAmount;
        user.Journeys = request.Journeys;
        await _userService.UpdateUserProfileAsync(user);
        return user;
    }
}