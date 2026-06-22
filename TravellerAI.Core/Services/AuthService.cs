using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;

namespace TravellerAI.Core.Services;

public class AuthService : IAuthService
{
    public Task<string> RegisterUser(RegisterUserModel user)
    {
        throw new NotImplementedException();
    }

    public Task<string> LoginUser(LoginUserModel user)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetUserProfile(AuthUserModel user)
    {
        throw new NotImplementedException();
    }
}    
