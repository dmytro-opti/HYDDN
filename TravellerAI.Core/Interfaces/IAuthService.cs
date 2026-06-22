using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels;
namespace TravellerAI.Core.Interfaces;

public interface IAuthService
{
    Task<string> RegisterUser(RegisterUserModel user);
    Task<string> LoginUser(LoginUserModel user);
    Task<string> GetUserProfile(AuthUserModel user);
}
