
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.Core.Interfaces;

public interface IValidationService
{
    Task<bool> ValidateEmail(LoginUserModel user);
    Task<bool> ValidatePassword(LoginUserModel user);
    Task<bool> ValidateDate(DateTime date);
    Task<bool> ValidateAccessibility();
    Task<bool> ValidateBirthDate(UserInfoModel user);
    Task<bool> CheckString(string stringToCheck);
    Task<bool> CheckInt(int intToCheck);
}
