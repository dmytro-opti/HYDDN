namespace TravellerAI.Domain.ViewModels.Responses;

public class AuthResponse
{
    /// <summary>JWT for the Authorization header: "Bearer {accessToken}".</summary>
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiresAt { get; set; }
    /// <summary>Single-use token for /api/auth/refresh, a new one is returned on every refresh.</summary>
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
    public AuthUserViewModel User { get; set; }
}
