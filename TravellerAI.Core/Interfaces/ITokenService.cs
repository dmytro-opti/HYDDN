using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Interfaces;

public interface ITokenService
{
    /// <summary>Signed short-lived access token (JWT) with user id, email and roles.</summary>
    TokenModel CreateAccessToken(AuthUserModel user);

    /// <summary>Random long-lived refresh token.</summary>
    TokenModel CreateRefreshToken();
}
