using AutoMapper;
using MediatR;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Features.Auth.RegisterUserCommand;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResultModel>
{
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    public Task<AuthResultModel> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        return _authService.RegisterAsync(_mapper.Map<RegisterUserModel>(command));
    }
}
