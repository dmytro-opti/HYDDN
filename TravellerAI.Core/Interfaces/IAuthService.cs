using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResultModel> RegisterAsync(RegisterUserModel user);
    Task<AuthResultModel> LoginAsync(LoginUserModel user);
    Task<AuthResultModel> RefreshAsync(string refreshToken);
    Task LogoutAsync(Guid userId);
    Task<AuthUserModel> GetCurrentUserAsync(Guid userId);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
    /// <summary>Deletes the account after the password confirmation.</summary>
    Task DeleteAccountAsync(Guid userId, string password);
}
