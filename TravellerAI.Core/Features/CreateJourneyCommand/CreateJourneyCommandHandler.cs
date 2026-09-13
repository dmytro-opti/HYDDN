using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Exceptions;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.CreateJourneyCommand;
//First stage
public class CreateJourneyCommandHandler : IRequestHandler<CreateJourneyCommand, Guid>
{
    private readonly IJourneyService _journeyService;
    private readonly IUserService _userService;
    
    public CreateJourneyCommandHandler(IJourneyService journeyService, IUserService userService)
    {
        _journeyService = journeyService;
        _userService = userService;
    }
    public async Task<Guid> Handle(CreateJourneyCommand command, CancellationToken cancellationToken)
    {
        return Guid.NewGuid();
    }
}

public interface IUserService
{
    public Task<UserModel> GetUserAsync(Guid userId);
}