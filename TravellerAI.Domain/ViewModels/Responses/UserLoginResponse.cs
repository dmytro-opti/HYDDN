namespace TravellerAI.Domain.ViewModels.Responses;

public class UserLoginResponse
{
    public string Token { get; set; }
    public UserViewModel User { get; set; }
}