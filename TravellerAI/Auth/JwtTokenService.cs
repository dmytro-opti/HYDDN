using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;
using TravellerAI.Settings;

namespace TravellerAI.Auth;

public class JwtTokenService : ITokenService
{
    private const int RefreshTokenBytes = 64;

    private readonly JwtSettings _settings;
    private readonly SigningCredentials _signingCredentials;
    private readonly JsonWebTokenHandler _tokenHandler = new();

    public JwtTokenService(IOptions<JwtSettings> settings)
    {
        _settings = settings.Value;
        _signingCredentials = new SigningCredentials(CreateSigningKey(_settings), SecurityAlgorithms.HmacSha256);
    }

    public TokenModel CreateAccessToken(AuthUserModel user)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_settings.AccessTokenMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.Name, user.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(user.Roles.Select(role => new Claim(AuthClaims.Role, role)));

        var token = _tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            Expires = expiresAt,
            SigningCredentials = _signingCredentials
        });

        return new TokenModel { Token = token, ExpiresAt = expiresAt };
    }

    public TokenModel CreateRefreshToken()
    {
        return new TokenModel
        {
            Token = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(RefreshTokenBytes)),
            ExpiresAt = DateTime.UtcNow.AddDays(_settings.RefreshTokenDays)
        };
    }

    public static SymmetricSecurityKey CreateSigningKey(JwtSettings settings)
    {
        if (string.IsNullOrEmpty(settings.SigningKey) || Encoding.UTF8.GetByteCount(settings.SigningKey) < JwtSettings.MinSigningKeyLength)
        {
            throw new InvalidOperationException(
                $"Jwt:SigningKey must be at least {JwtSettings.MinSigningKeyLength} bytes. " +
                "Set it with 'dotnet user-secrets set Jwt:SigningKey <key>' or environment variable Jwt__SigningKey.");
        }

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey));
    }
}
