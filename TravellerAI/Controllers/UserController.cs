using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Auth;
using TravellerAI.Core.Features.GetUserProfileCommand;
using TravellerAI.Core.Features.UpdateUserProfileCommand;
using TravellerAI.Core.Features.User.DeleteAccountCommand;
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
[Authorize]
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
    /// Returns travel preferences of the current user.
    /// </summary>
    [HttpGet("me/profile")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileViewModel>> GetProfile(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var profile = await _mediator.Send(new GetUserProfileCommand { UserId = userId }, cancellationToken);

        return _mapper.Map<UserProfileViewModel>(profile);
    }

    /// <summary>
    /// Updates names and travel preferences of the current user. Email and password have their own endpoints.
    /// </summary>
    [HttpPut("me/profile")]
    [ProducesResponseType(typeof(UserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<UserViewModel>> UpdateProfile([FromBody] UpdateUserProfileRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateUserProfileCommand>(request);
        command.UserId = User.GetUserId();

        var user = await _mediator.Send(command, cancellationToken);

        return _mapper.Map<UserViewModel>(user);
    }

    /// <summary>
    /// Changes email (also the login) of the current user. The new email has to be confirmed again.
    /// </summary>
    [HttpPut("me/email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateUserEmailRequest request,
        CancellationToken cancellationToken)
    {
        var command = _mapper.Map<UpdateUserEmailCommand>(request);
        command.UserId = User.GetUserId();

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Deletes the account and all user data. The current password is required.
    /// </summary>
    [HttpDelete("me")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteAccountCommand { UserId = User.GetUserId(), Password = request.Password }, cancellationToken);

        return NoContent();
    }
}
