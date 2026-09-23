using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Core.Features.GetUserProfileCommand;
using TravellerAI.Core.Features.UpdateUserProfileCommand;
using TravellerAI.Core.Features.User.UpdateUserEmailCommand;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Users and their profiles.
/// </summary>
/// <remarks>
/// Exceptions are translated to HTTP responses by GlobalExceptionHandler
/// (validation - 400, forbidden - 403, not found - 404, conflict - 409).
/// </remarks>
[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public UserController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Returns user profile info (preferences, interests, languages).
    /// </summary>
    [HttpGet("{userId:guid}/profile")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileViewModel>> GetProfile(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await _mediator.Send(new GetUserProfileCommand { UserId = userId }, cancellationToken);

        return _mapper.Map<UserProfileViewModel>(profile);
    }

    /// <summary>
    /// Updates user name, email and profile preferences. Password is changed separately.
    /// </summary>
    [HttpPut("{userId:guid}/profile")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserViewModel>> UpdateProfile(Guid userId, [FromBody] UpdateUserProfileRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateUserProfileCommand>(request);
        command.UserId = userId;

        var user = await _mediator.Send(command, cancellationToken);

        return _mapper.Map<UserViewModel>(user);
    }

    /// <summary>
    /// Changes user email. The new email has to be confirmed again.
    /// </summary>
    [HttpPut("{userId:guid}/email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateEmail(Guid userId, [FromBody] UpdateUserEmailRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateUserEmailCommand>(request);
        command.UserId = userId;

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
