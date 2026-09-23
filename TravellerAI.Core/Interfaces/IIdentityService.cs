using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

/// <summary>
/// User accounts and credentials (implemented with ASP.NET Core Identity).
/// </summary>
public interface IIdentityService
{
    /// <summary>Creates identity user and its profile with the same Id.</summary>
    Task<AuthUserModel> CreateUserAsync(RegisterUserModel model);

    /// <summary>Returns the user when the credentials are valid, otherwise null. Handles lockout.</summary>
    Task<AuthUserModel?> ValidateCredentialsAsync(string email, string password);

    Task<AuthUserModel> GetUserAsync(Guid userId);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    /// <summary>Changes identity email (login) and the profile email copy. Email has to be confirmed again.</summary>
    Task ChangeEmailAsync(Guid userId, string email);
    /// <summary>Deletes identity user together with the profile.</summary>
    Task DeleteUserAsync(Guid userId);

    Task StoreRefreshTokenAsync(Guid userId, TokenModel refreshToken);
    /// <summary>Validates and revokes the refresh token (rotation). Returns the token owner.</summary>
    Task<Guid> RedeemRefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokensAsync(Guid userId);
}
