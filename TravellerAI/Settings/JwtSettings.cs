namespace TravellerAI.Settings;

/// <summary>
/// "Jwt" configuration section. SigningKey is a secret: set it with user-secrets or environment variable Jwt__SigningKey.
/// </summary>
public class JwtSettings
{
    public const string SectionName = "Jwt";
    /// <summary>HMAC-SHA256 key has to be at least 256 bits.</summary>
    public const int MinSigningKeyLength = 32;

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 14;
}
