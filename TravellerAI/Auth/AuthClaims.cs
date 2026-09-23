using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using TravellerAI.Core.Exceptions;

namespace TravellerAI.Auth;

public static class AuthClaims
{
    /// <summary>JWT claims are not mapped to long .NET claim types (MapInboundClaims = false).</summary>
    public const string Role = "role";

    /// <summary>
    /// Id of the authenticated user (JWT "sub" claim).
    /// </summary>
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(value, out var userId)
            ? userId
            : throw new UnauthorizedException("User is not authenticated");
    }
}
