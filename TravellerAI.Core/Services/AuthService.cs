using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Enums;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class AuthService : IAuthService
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;
    private readonly IValidationService _validationService;
    private readonly INotificationService _notificationService;
    private readonly ILoggerService<AuthService> _logger;

    public AuthService(IIdentityService identityService, ITokenService tokenService, IValidationService validationService,
        INotificationService notificationService, ILoggerService<AuthService> logger)
    {
        _identityService = identityService;
        _tokenService = tokenService;
        _validationService = validationService;
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<AuthResultModel> RegisterAsync(RegisterUserModel user)
    {
        var credentials = new LoginUserModel { Email = user.Email, Password = user.Password };
        if (!await _validationService.ValidateEmail(credentials) || !await _validationService.ValidatePassword(credentials))
        {
            throw new BadRequestException("Email or password does not meet the requirements");
        }

        var created = await _identityService.CreateUserAsync(user);
        _logger.Log(ErrorLevel.Low, $"User {created.Id} was registered");

        await _notificationService.AddNotificationAsync(created.Id, NotificationType.Info,
            "Welcome to TravellerAI", "Fill in your travel profile to get better recommendations.");

        return await IssueTokensAsync(created);
    }

    public async Task<AuthResultModel> LoginAsync(LoginUserModel user)
    {
        var authenticated = await _identityService.ValidateCredentialsAsync(user.Email, user.Password);
        if (authenticated == null)
        {
            // the same message for unknown email and wrong password
            throw new UnauthorizedException("Invalid email or password");
        }

        _logger.Log(ErrorLevel.Low, $"User {authenticated.Id} logged in");

        return await IssueTokensAsync(authenticated);
    }

    public async Task<AuthResultModel> RefreshAsync(string refreshToken)
    {
        var userId = await _identityService.RedeemRefreshTokenAsync(refreshToken);
        var user = await _identityService.GetUserAsync(userId);

        return await IssueTokensAsync(user);
    }

    public async Task LogoutAsync(Guid userId)
    {
        await _identityService.RevokeRefreshTokensAsync(userId);
        _logger.Log(ErrorLevel.Low, $"User {userId} logged out");
    }

    public Task<AuthUserModel> GetCurrentUserAsync(Guid userId)
    {
        return _identityService.GetUserAsync(userId);
    }

    public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        if (currentPassword == newPassword)
        {
            throw new BadRequestException("New password must differ from the current one");
        }

        await _identityService.ChangePasswordAsync(userId, currentPassword, newPassword);
        // sign out other sessions
        await _identityService.RevokeRefreshTokensAsync(userId);

        await _notificationService.AddNotificationAsync(userId, NotificationType.Security,
            "Password changed", "Your password was changed. All other sessions were signed out.");
        _logger.Log(ErrorLevel.Low, $"User {userId} password was changed");
    }

    public async Task DeleteAccountAsync(Guid userId, string password)
    {
        var user = await _identityService.GetUserAsync(userId);

        if (await _identityService.ValidateCredentialsAsync(user.Email, password) == null)
        {
            throw new BadRequestException("Password is incorrect");
        }

        await _identityService.DeleteUserAsync(userId);
        _logger.Log(ErrorLevel.Low, $"User {userId} deleted the account");
    }

    private async Task<AuthResultModel> IssueTokensAsync(AuthUserModel user)
    {
        var refreshToken = _tokenService.CreateRefreshToken();
        await _identityService.StoreRefreshTokenAsync(user.Id, refreshToken);

        return new AuthResultModel
        {
            AccessToken = _tokenService.CreateAccessToken(user),
            RefreshToken = refreshToken,
            User = user
        };
    }
}
