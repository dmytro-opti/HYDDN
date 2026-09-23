namespace TravellerAI.Domain.Models;

public class AuthResultModel
{
    public TokenModel AccessToken { get; set; }
    public TokenModel RefreshToken { get; set; }
    public AuthUserModel User { get; set; }
}
