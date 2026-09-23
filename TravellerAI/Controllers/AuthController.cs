using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravellerAI.Auth;
using TravellerAI.Core.Features.Auth.ChangePasswordCommand;
using TravellerAI.Core.Features.Auth.GetCurrentUserCommand;
using TravellerAI.Core.Features.Auth.LoginUserCommand;
using TravellerAI.Core.Features.Auth.LogoutCommand;
using TravellerAI.Core.Features.Auth.RefreshTokenCommand;
using TravellerAI.Core.Features.Auth.RegisterUserCommand;
using TravellerAI.Domain.ViewModels;
using TravellerAI.Domain.ViewModels.Requests;
using TravellerAI.Domain.ViewModels.Responses;

namespace TravellerAI.WebApi.Controllers;

/// <summary>
/// Registration, login and JWT tokens.
/// </summary>
/// <remarks>
/// Exceptions are translated to HTTP responses by GlobalExceptionHandler
/// (validation - 400, invalid credentials / token - 401, email taken - 409).
/// </remarks>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Registers a new user and signs them in.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<RegisterUserCommand>(request), cancellationToken);

        return CreatedAtAction(nameof(Me), _mapper.Map<AuthResponse>(result));
    }

    /// <summary>
    /// Signs in with email and password. The account is locked after several failed attempts.
    /// </summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<LoginUserCommand>(request), cancellationToken);

        return _mapper.Map<AuthResponse>(result);
    }

    /// <summary>
    /// Exchanges a refresh token for new access and refresh tokens (the used refresh token is revoked).
    /// </summary>
    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(_mapper.Map<RefreshTokenCommand>(request), cancellationToken);

        return _mapper.Map<AuthResponse>(result);
    }

    /// <summary>
    /// Signs out all sessions of the user (revokes refresh tokens).
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        await _mediator.Send(new LogoutCommand { UserId = User.GetUserId() }, cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Returns the authenticated user.
    /// </summary>
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(AuthUserViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthUserViewModel>> Me(CancellationToken cancellationToken)
    {
        var user = await _mediator.Send(new GetCurrentUserCommand { UserId = User.GetUserId() }, cancellationToken);

        return _mapper.Map<AuthUserViewModel>(user);
    }

    /// <summary>
    /// Changes the password and signs out other sessions.
    /// </summary>
    [Authorize]
    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<ChangePasswordCommand>(request);
        command.UserId = User.GetUserId();

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}
