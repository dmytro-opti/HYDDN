namespace TravellerAI.Domain.Models;

public class LoginUserModel
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
}