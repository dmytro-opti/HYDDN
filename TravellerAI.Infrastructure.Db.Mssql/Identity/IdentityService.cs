using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravellerAI.Core;
using TravellerAI.Core.Exceptions;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Entities;
using TravellerAI.Domain.Models;
using TravellerAI.Infrastructure.Db.Mssql.Context;

namespace TravellerAI.Infrastructure.Db.Mssql.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TravellerDbContext _context;

    public IdentityService(UserManager<ApplicationUser> userManager, TravellerDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<AuthUserModel> CreateUserAsync(RegisterUserModel model)
    {
        var email = model.Email.Trim();
        var id = Guid.NewGuid();

        var user = new ApplicationUser
        {
            Id = id,
            UserName = email,
            Email = email,
            // saved together with the identity user
            Profile = new UserEntity
            {
                Id = id,
                Name = model.Name.Trim(),
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                Email = email
            }
        };

        await using var transaction = await _context.Database.BeginTransactionAsync();

        ThrowIfFailed(await _userManager.CreateAsync(user, model.Password));
        ThrowIfFailed(await _userManager.AddToRoleAsync(user, Constants.Security.UserRole));

        await transaction.CommitAsync();

        return await ToModelAsync(user);
    }

    public async Task<AuthUserModel?> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email.Trim());
        if (user == null)
        {
            return null;
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            throw new UnauthorizedException(
                $"Account is locked after too many failed attempts, try again in {Constants.Security.LockoutMinutes} minutes");
        }

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            // counts failed attempts, locks the account after MaxFailedAccessAttempts
            await _userManager.AccessFailedAsync(user);
            return null;
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        return await ToModelAsync(user);
    }

    public async Task<AuthUserModel> GetUserAsync(Guid userId)
    {
        return await ToModelAsync(await GetIdentityUserAsync(userId));
    }

    public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await GetIdentityUserAsync(userId);
        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

        if (result.Errors.Any(e => e.Code == nameof(IdentityErrorDescriber.PasswordMismatch)))
        {
            throw new BadRequestException("Current password is incorrect");
        }

        ThrowIfFailed(result);
    }

    public async Task ChangeEmailAsync(Guid userId, string email)
    {
        email = email.Trim();
        var user = await GetIdentityUserAsync(userId);

        var owner = await _userManager.FindByEmailAsync(email);
        if (owner != null && owner.Id != userId)
        {
            throw new ConflictException($"Email {email} is already in use");
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();

        if (user.Profile != null)
        {
            user.Profile.Email = email;
        }

        // SetEmailAsync resets EmailConfirmed; email is also the login name
        ThrowIfFailed(await _userManager.SetEmailAsync(user, email));
        ThrowIfFailed(await _userManager.SetUserNameAsync(user, email));

        await transaction.CommitAsync();
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var user = await GetIdentityUserAsync(userId);

        // the profile and user data are removed by database cascade
        ThrowIfFailed(await _userManager.DeleteAsync(user));
    }

    public async Task StoreRefreshTokenAsync(Guid userId, TokenModel refreshToken)
    {
        // housekeeping: expired tokens of the user are not needed anymore
        await _context.RefreshTokens
            .Where(t => t.UserId == userId && t.ExpiresAt < DateTime.UtcNow)
            .ExecuteDeleteAsync();

        _context.RefreshTokens.Add(new RefreshTokenEntity
        {
            UserId = userId,
            TokenHash = Hash(refreshToken.Token),
            ExpiresAt = refreshToken.ExpiresAt
        });

        await _context.SaveChangesAsync();
    }

    public async Task<Guid> RedeemRefreshTokenAsync(string refreshToken)
    {
        var hash = Hash(refreshToken);
        var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.TokenHash == hash)
                    ?? throw new UnauthorizedException("Invalid refresh token");

        if (token.RevokedAt != null)
        {
            // a revoked token is used again - it may be stolen, sign out all sessions
            await RevokeRefreshTokensAsync(token.UserId);
            throw new UnauthorizedException("Refresh token has already been used");
        }

        if (token.ExpiresAt <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token has expired");
        }

        token.RevokedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return token.UserId;
    }

    public async Task RevokeRefreshTokensAsync(Guid userId)
    {
        var now = DateTime.UtcNow;

        await _context.RefreshTokens
            .Where(t => t.UserId == userId && t.RevokedAt == null)
            .ExecuteUpdateAsync(s => s
                .SetProperty(t => t.RevokedAt, now)
                .SetProperty(t => t.Updated, now));
    }

    private async Task<ApplicationUser> GetIdentityUserAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString())
               ?? throw new NotFoundException("User", userId);
    }

    private async Task<AuthUserModel> ToModelAsync(ApplicationUser user)
    {
        return new AuthUserModel
        {
            Id = user.Id,
            Email = user.Email!,
            EmailConfirmed = user.EmailConfirmed,
            Name = user.Profile?.Name ?? string.Empty,
            FirstName = user.Profile?.FirstName ?? string.Empty,
            LastName = user.Profile?.LastName ?? string.Empty,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };
    }

    private static void ThrowIfFailed(IdentityResult result)
    {
        if (result.Succeeded)
        {
            return;
        }

        if (result.Errors.Any(e => e.Code is nameof(IdentityErrorDescriber.DuplicateEmail)
                or nameof(IdentityErrorDescriber.DuplicateUserName)))
        {
            throw new ConflictException("Email is already registered");
        }

        throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));
    }

    private static string Hash(string token) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
