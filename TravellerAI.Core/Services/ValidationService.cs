using TravellerAI.Core.Interfaces;
using TravellerAI.Domain.Models;
using TravellerAI.Domain.ViewModels.Requests;

namespace TravellerAI.Core.Services;

public class ValidationService : IValidationService
{
    public Task<bool> ValidateEmail(LoginUserModel user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidatePassword(LoginUserModel user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidateDate(DateTime date)
    {
        throw new NotImplementedException();
    }
   
    public Task<bool> ValidateAccessibility()
    {
        throw new NotImplementedException();
    }
    public Task<bool> ValidateBirthDate(UserInfoModel user)
    {
        throw new NotImplementedException();
    }
    public Task<bool> CheckString(string stringToCheck)
    {
        throw new NotImplementedException();
    }
    public Task<bool> CheckPeriod (PeriodModel period)
    {
        throw new NotImplementedException();
    }
    public Task<bool> CheckInt(int intToCheck)
    {
        throw new NotImplementedException();
    }
}
